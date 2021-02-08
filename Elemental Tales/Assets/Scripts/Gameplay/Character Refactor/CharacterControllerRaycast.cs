using UnityEngine;
using System.Collections;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
 *    
 *    Controls all movement logic of the player. Takes commands from the PlayerInputs script, and translates the player based on the command and the players current location and activities.
 */

[RequireComponent(typeof(CameraWork))]
public class CharacterControllerRaycast : RaycastController
{
	public float maxSlopeAngle = 80;

	public CollisionInfo collisions;
	[HideInInspector]
	public Vector2 playerInput;
	public bool isPlayer;
	public bool isPushable;
	public bool debugEnabled = true;
	
	/// <summary>
	/// Overrides the start method found in RaycastController and initialises CameraWork and direction.
	/// </summary>
	public override void Start()
	{
		base.Start();
		if (isPlayer)
		{
			CameraWork();
		}
		collisions.faceDir = 1;
	}

	/// <summary>
	/// Returns the result of the move method where there is no input.
	/// </summary>
	/// <param name="moveAmount"></param>
	/// <param name="standingOnPlatform"></param>
	/// <returns></returns>
	public Vector2 Move(Vector2 moveAmount, bool standingOnPlatform)
	{
		return Move(moveAmount, Vector2.zero, standingOnPlatform);
	}

	/// <summary>
	/// Performs the movement logic of the object, given an input and the "speed" of the object. Checks for horizontal and vertical collisions and returns the
	/// resulting total movement vector.
	/// </summary>
	/// <param name="moveAmount"></param>
	/// <param name="input"></param>
	/// <param name="standingOnPlatform"></param>
	/// <returns></returns>
	public Vector2 Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false)
	{
		UpdateRaycastOrigins();

		collisions.Reset();
		collisions.moveAmountOld = moveAmount;
		collisions.wasDisplayingHint = collisions.shouldDisplayHint;
		playerInput = input;

		if (moveAmount.y < 0)
		{
			DescendSlope(ref moveAmount);
		}

		if (moveAmount.x != 0)
		{
			collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
		}

		HorizontalCollisions(ref moveAmount);
		if (moveAmount.y != 0)
		{
			VerticalCollisions(ref moveAmount);
		}

		transform.Translate(moveAmount);

		if (standingOnPlatform)
		{
			collisions.below = true;
		}

		return moveAmount;
	}

	/// <summary>
	/// Checks for collisions the player will encounter given the horizontal movement needed.
	/// </summary>
	/// <param name="moveAmount"></param>
	void HorizontalCollisions(ref Vector2 moveAmount)
	{
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
		int hintBoolCounter = 0;

		if (Mathf.Abs(moveAmount.x) < skinWidth)
		{
			rayLength = 2 * skinWidth;
		}

		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			Vector2 rayOriginOpposite = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			rayOriginOpposite += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			RaycastHit2D hitOpposite = Physics2D.Raycast(rayOriginOpposite, Vector2.zero, 0, collisionMask);

			if (collisions.isPulling)
			{
				hitOpposite = Physics2D.Raycast(rayOriginOpposite, Vector2.right * -directionX, 5, collisionMask);
			}

			if(debugEnabled)
				Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

			if (hit || hitOpposite)
			{
				if(hitOpposite)
                {
					if (hitOpposite.collider.CompareTag("Pushable"))
					{
						collisions.isHoldingObject = true;
						if (collisions.isPulling)
						{
							collisions.isHoldingObject = true;
							hitOpposite.collider.GetComponent<PushableObject>().Push(moveAmount);
						}
					}
				}
				
				if (hit)
				{
					if (hit.distance == 0)
					{
						continue;
					}

					if (hit.collider.CompareTag("Pushable"))
					{
						collisions.isHoldingObject = true;
						if (collisions.isPulling)
						{
							collisions.isHoldingObject = true;
							hit.collider.GetComponent<PushableObject>().Push(moveAmount);
						}
					}

					float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

					if (i == 0 && slopeAngle <= maxSlopeAngle)
					{
						if (collisions.descendingSlope)
						{
							collisions.descendingSlope = false;
							moveAmount = collisions.moveAmountOld;
						}
						float distanceToSlopeStart = 0;
						if (slopeAngle != collisions.slopeAngleOld)
						{
							distanceToSlopeStart = hit.distance - skinWidth;
							moveAmount.x -= distanceToSlopeStart * directionX;
						}
						ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
						moveAmount.x += distanceToSlopeStart * directionX;
					}

					if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
					{
						moveAmount.x = (hit.distance - skinWidth) * directionX;
						rayLength = hit.distance;

						if (collisions.climbingSlope)
						{
							moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
						}

						collisions.left = directionX == -1;
						collisions.right = directionX == 1;
					}
				}
			}

			if (isPushable)
			{
				RaycastHit2D hintHit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, directionX * 2, collisionMask);
				RaycastHit2D hintHitOpposite = Physics2D.Raycast(rayOriginOpposite, Vector2.right * -directionX, directionX * 2, collisionMask);
				if (debugEnabled)
				{
					Debug.DrawRay(rayOrigin, Vector2.right * directionX * 2, Color.yellow);
					Debug.DrawRay(rayOriginOpposite, Vector2.right * -directionX * 2, Color.yellow);
				}

				if (((hintHit && hintHit.collider.gameObject.GetPhotonView().IsMine) || !PhotonNetwork.IsConnected) || ((hintHitOpposite && hintHitOpposite.collider.gameObject.GetPhotonView().IsMine) || !PhotonNetwork.IsConnected))
				{
					hintBoolCounter += 1;
				}
			}
		}

		if(hintBoolCounter >= 1)
        {
			hintBoolCounter = 0;
			if(!collisions.wasDisplayingHint)
            {
				collisions.shouldDisplayHint = true;
			}
        } else if(collisions.wasDisplayingHint)
        {
			collisions.shouldDisplayHint = false;
		}
	}

	/// <summary>
	/// Checks for collisions which will occur given vertical movement.
	/// </summary>
	/// <param name="moveAmount"></param>
	void VerticalCollisions(ref Vector2 moveAmount)
	{
		float directionY = Mathf.Sign(moveAmount.y);
		float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++)
		{

			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

			if (hit)
			{
				if (hit.collider.tag == "Permeable")
				{
					collisions.isOnPermeable = true;
					if (directionY == 1 || hit.distance == 0)
					{
						continue;
					}
					if (collisions.fallingThroughPlatform)
					{
						continue;
					}
					if (playerInput.y == -1 && Input.GetKey(KeyCode.Space))
					{
						collisions.fallingThroughPlatform = true;
						Invoke("ResetFallingThroughPlatform", .5f);
						continue;
					}
				}

				moveAmount.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if (collisions.climbingSlope)
				{
					moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}

		if (collisions.climbingSlope)
		{
			float directionX = Mathf.Sign(moveAmount.x);
			rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			if (hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != collisions.slopeAngle)
				{
					moveAmount.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
					collisions.slopeNormal = hit.normal;
				}
			}
		}
	}

	/// <summary>
	/// Checks whether the object is trying to climb a slope, and returns the modified movement vector to allow for full movement up an incline, given a minimum and maximum slope.
	/// </summary>
	/// <param name="moveAmount"></param>
	/// <param name="slopeAngle"></param>
	/// <param name="slopeNormal"></param>
	void ClimbSlope(ref Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal)
	{
		float moveDistance = Mathf.Abs(moveAmount.x);
		float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (moveAmount.y <= climbmoveAmountY)
		{
			moveAmount.y = climbmoveAmountY;
			moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
			collisions.slopeNormal = slopeNormal;
		}
	}

	/// <summary>
	/// Checks whether the object is trying to descend a slope, and returns the modified movement vector to allow for full movement up an incline, given a minimum and maximum slope.
	/// </summary>
	/// <param name="moveAmount"></param>
	void DescendSlope(ref Vector2 moveAmount)
	{

		RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + skinWidth, collisionMask);
		RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + skinWidth, collisionMask);
		if (maxSlopeHitLeft ^ maxSlopeHitRight)
		{
			SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmount);
			SlideDownMaxSlope(maxSlopeHitRight, ref moveAmount);
		}

		if (!collisions.slidingDownMaxSlope)
		{
			float directionX = Mathf.Sign(moveAmount.x);
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

			if (hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
				{
					if (Mathf.Sign(hit.normal.x) == directionX)
					{
						if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
						{
							float moveDistance = Mathf.Abs(moveAmount.x);
							float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
							moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
							moveAmount.y -= descendmoveAmountY;

							collisions.slopeAngle = slopeAngle;
							collisions.descendingSlope = true;
							collisions.below = true;
							collisions.slopeNormal = hit.normal;
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Modifies the movement vector to override player input and slide them down a slope if the incline is too steep.
	/// </summary>
	/// <param name="hit"></param>
	/// <param name="moveAmount"></param>
	void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount)
	{

		if (hit)
		{
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle > maxSlopeAngle)
			{
				moveAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

				collisions.slopeAngle = slopeAngle;
				collisions.slidingDownMaxSlope = true;
				collisions.slopeNormal = hit.normal;
			}
		}

	}

	/// <summary>
	/// Resets the bool for the object falling through a platform.
	/// </summary>
	void ResetFallingThroughPlatform()
	{
		collisions.fallingThroughPlatform = false;
	}

	/// <summary>
	/// Initialises the camera to this object if and only if this object belongs to the player.
	/// </summary>
	void CameraWork()
	{
		CameraWork _cameraWork = GetComponent<CameraWork>();

		if (_cameraWork != null)
		{
			if (photonView.IsMine || !PhotonNetwork.IsConnected)
			{
				GameObject.Find("Game Manager").GetComponent<GameMaster>().setPlayer(this.gameObject);
				//Debug.Log("Following player now");
				_cameraWork.OnStartFollowing();
			} else
            {
				print("Photonview is not mine");
            }
		}
		else
		{
			Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
		}
	}

	/// <summary>
	/// Stores public information about the object, allowing other classes to directly access and modify object variables. Resets the variables every tick.
	/// </summary>
	public struct CollisionInfo
	{
		public bool above, below;
		public bool left, right;

		public bool climbingSlope;
		public bool descendingSlope;
		public bool slidingDownMaxSlope;

		public float slopeAngle, slopeAngleOld;
		public Vector2 slopeNormal;
		public Vector2 moveAmountOld;
		public int faceDir;
		public bool fallingThroughPlatform;
		public bool isOnPermeable;

		public bool isPulling;
		public bool isHoldingObject;

		public bool shouldDisplayHint;
		public bool wasDisplayingHint;

		public void Reset()
		{
			above = below = false;
			left = right = false;
			climbingSlope = false;
			descendingSlope = false;
			slidingDownMaxSlope = false;
			slopeNormal = Vector2.zero;
			isOnPermeable = false;
			isHoldingObject = false;

			wasDisplayingHint = shouldDisplayHint;

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}

}
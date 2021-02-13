using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.2.2
 *    @version 0.0.0
 *    
 *    Controller for a block which will try to fall on players, before raising back into their start position. Being crushed by the block will kill the player instantly, in either direction.
 */
[RequireComponent(typeof(CharacterControllerRaycast))]
public class FallingBlock : RaycastController
{
	public LayerMask passengerMask;

	public float gravity = 40;
    public float raiseSpeed = 12;
    CharacterControllerRaycast controller;
    Vector3 velocity;
    bool isFalling;

	List<PassengerMovement> passengerMovement;
	Dictionary<Transform, CharacterControllerRaycast> passengerDictionary = new Dictionary<Transform, CharacterControllerRaycast>();

	public override void Start()
    {
        controller = GetComponent<CharacterControllerRaycast>();
    }

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            isFalling = !isFalling;
        }

		UpdateRaycastOrigins();

		// Perform gravity and collisions check
		if (isFalling)
        {
            velocity += Vector3.down * gravity * Time.deltaTime;
            //controller.Move(velocity * Time.deltaTime, false);
        }
        if (controller.collisions.below)
        {
            velocity = Vector3.zero;
        }
        if(!isFalling)
        {
            velocity += Vector3.up * raiseSpeed * Time.deltaTime;
            //controller.Move(velocity * Time.deltaTime, false);
            if(controller.collisions.above)
            {
                velocity = Vector3.zero;
            }
        }

		CalculatePassengerMovement(velocity * Time.deltaTime);
		MovePassengers(true);
		controller.Move(velocity * Time.deltaTime, false);
		MovePassengers(false);
	}

    public void TriggerFalling()
    {
		photonView.RPC("TriggerBlockFall", RpcTarget.AllBuffered);
	}

	[PunRPC] private void TriggerBlockFall()
	{
		StartCoroutine(WaitToResetBlock());
	}

	IEnumerator WaitToResetBlock()
    {
		isFalling = true;
		while(!controller.collisions.below)
        {
			yield return new WaitForFixedUpdate();
        }
		yield return new WaitForSeconds(1.5f);
		isFalling = false;
    }

	void MovePassengers(bool beforeMovePlatform)
	{
		foreach (PassengerMovement passenger in passengerMovement)
		{
			if (!passengerDictionary.ContainsKey(passenger.transform))
			{
				passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<CharacterControllerRaycast>());
			}

			if (passenger.moveBeforePlatform == beforeMovePlatform)
			{
				passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
			}
		}
	}

	/// <summary>
	/// Logic for calculating how the passengers should move based off the movement of the platform
	/// </summary>
	/// <param name="velocity"></param>
	void CalculatePassengerMovement(Vector3 velocity)
	{
		HashSet<Transform> movedPassengers = new HashSet<Transform>();
		passengerMovement = new List<PassengerMovement>();

		float directionX = Mathf.Sign(velocity.x);
		float directionY = Mathf.Sign(velocity.y);

		// Vertically moving platform
		if (velocity.y != 0)
		{
			float rayLength = Mathf.Abs(velocity.y) + skinWidth;

			for (int i = 0; i < verticalRayCount; i++)
			{
				Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

				if (hit && hit.distance != 0)
				{
					if (!movedPassengers.Contains(hit.transform))
					{
						movedPassengers.Add(hit.transform);
						float pushX = (directionY == 1) ? velocity.x : 0;
						float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

						passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
					}
				}
			}
		}

		// Passenger on top of a horizontally or downward moving platform
		if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
		{
			float rayLength = skinWidth * 2;

			for (int i = 0; i < verticalRayCount; i++)
			{
				Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

				if (hit && hit.distance != 0)
				{
					if (!movedPassengers.Contains(hit.transform))
					{
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;

						passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
					}
				}
			}
		}
	}

	/// <summary>
	/// Contains variables for the movement of the passengers standing on the platform.
	/// </summary>
	struct PassengerMovement
	{
		public Transform transform;
		public Vector3 velocity;
		public bool standingOnPlatform;
		public bool moveBeforePlatform;

		public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
		{
			transform = _transform;
			velocity = _velocity;
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform;
		}
	}
}

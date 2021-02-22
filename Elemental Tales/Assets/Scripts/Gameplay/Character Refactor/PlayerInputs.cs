using UnityEngine;
using System.Collections;
using Cinemachine;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 3.0.0
 *    
 *    Accepts player input and converts it to directions to be sent to the controller class. Sets up the physical simulation parameters of the object.
 */

[RequireComponent(typeof(CharacterControllerRaycast))]
[RequireComponent(typeof(StatController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputs : MonoBehaviour
{
	[Header("Jump Variables")]
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	public int numberOfJumps = 2;
	int currentNumberOfJumps;
	bool isJumping;

	[Header("Movement Variables")]
	public float moveSpeed = 12;

	[Header("Wallclimb Variables")]
	[HideInInspector]
	public Vector2 wallJumpClimb;
	[HideInInspector]
	public Vector2 wallJumpOff;
	[HideInInspector]
	public Vector2 wallLeap;

	[Header("Wallclimb Variables")]
	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	[Header("Dash Variables")]
	public float dashSpeed = 60;
	public int numberOfDashes = 2;
	int currentNumberOfDashes;

	[Header("Smash Variables")]
	public float smashSpeed = -35f;
	public GameObject slamParticles;
	bool isSmashing;

	[Header("Bash Variables")]
	[SerializeField] float radius;
	[SerializeField] GameObject bashableObj;
	bool nearToBashableObj;
	bool isChoosingDir;
	bool isBashing;
	bool isMidBash;
	float lastAngle;
	public float bashPower = 60;
	GameObject arrow;

	[Header("Attack Variables")]
	[SerializeField] private Animator animator;

	[SerializeField] private Transform attackPoint;
	[SerializeField] private float attackRange = 0.5f;
	[SerializeField] private LayerMask enemyLayers;

	public Animator camAnim;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	[HideInInspector]
	public CharacterControllerRaycast controller;
	ElementController elementController;

	Vector2 directionalInput;
	bool wallSliding;
	int wallDirX;
	bool isHoldingObject;

	CinemachineFramingTransposer vCam;

	bool elementControllerHasInstantiated = false;

	/// <summary>
	/// Initialises the player components and physical parameters.
	/// </summary>
	void Start()
	{
		currentNumberOfJumps = numberOfJumps;
		currentNumberOfDashes = numberOfDashes;
		controller = GetComponent<CharacterControllerRaycast>();
		elementController = GetComponent<ElementController>();
		elementControllerHasInstantiated = true;

		arrow = GameObject.Find("Game Manager").GetComponent<GameMaster>().arrow;
		vCam = GameObject.Find("Virtual Camera").GetComponentInChildren<CinemachineFramingTransposer>();
		camAnim = GameObject.Find("Virtual Camera").GetComponentInChildren<Animator>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

    /// <summary>
    /// Calculates and sends off the current desired/expected velocity, taking into account obstacles, gravity and player input.
    /// </summary>
    void Update()
    {
		if (controller.collisions.below)
        {
			currentNumberOfJumps = numberOfJumps;
			currentNumberOfDashes = numberOfDashes;
			if(isJumping)
            {
				isJumping = false;
			}
			
			if(isSmashing)
            {
				isSmashing = false;
				camAnim.SetTrigger("Trigger");
				Instantiate(slamParticles, new Vector3(transform.position.x + .0375f, transform.position.y, transform.position.z), Quaternion.identity);
				GetComponent<PlayerInput>().hasControl = true;
			}
			if(isMidBash)
            {
				isMidBash = false;
            }
		} 

		if(isMidBash && directionalInput.x == 0)
        {
			directionalInput.x = lastAngle;
        }

		CalculateVelocity();
        HandleWallSliding();

        if (controller.collisions.below && directionalInput.y == -1)
        {
            controller.Move(velocity * Time.deltaTime * 0.5f, directionalInput);
            return;
        }

		if(isSmashing)
        {
			velocity.x = 0;
        }

		if(isBashing)
        {
			velocity.x = 0;
			velocity.y = 0;
        }

		OnSlingshotInputDown();

		controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }

		for (int i = 0; i < controller.horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (controller.collisions.faceDir == -1) ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (controller.horizontalRaySpacing * i);

			Debug.DrawRay(rayOrigin, Vector2.right * controller.collisions.faceDir * 2, Color.red);
		}
	}

	public void PanCamDownKeyDown()
    {
		StopCoroutine(PanUpCam());
		StopCoroutine(ReturnCamToCentreFromPanUp());
		StopCoroutine(ReturnCamToCentreFromPanDown());
		StartCoroutine(PanDownCam());
    }

	public void PanCamDownKeyUp()
    {
		StopCoroutine(PanDownCam());
		StartCoroutine(ReturnCamToCentreFromPanDown());
    }

	public void PanCamUpKeyDown()
    {
		StopCoroutine(PanDownCam());
		StopCoroutine(ReturnCamToCentreFromPanUp());
		StopCoroutine(ReturnCamToCentreFromPanDown());
		StartCoroutine(PanUpCam());
	}

	public void PanCamUpKeyUp()
	{
		StopCoroutine(PanUpCam());
		StartCoroutine(ReturnCamToCentreFromPanUp());
	}

	IEnumerator PanDownCam()
    {
		while (vCam.m_ScreenY > .25f)
        {
			yield return new WaitForFixedUpdate();
			vCam.m_ScreenY -= .005f;
        }
    }

	IEnumerator ReturnCamToCentreFromPanDown()
    {
		while (vCam.m_ScreenY < .5f)
        {
			yield return new WaitForFixedUpdate();
			vCam.m_ScreenY += .005f;
        }
    }

	IEnumerator PanUpCam()
    {
		while (vCam.m_ScreenY < .75f)
        {
			yield return new WaitForFixedUpdate();
			vCam.m_ScreenY += .005f;
        }
    }

	IEnumerator ReturnCamToCentreFromPanUp()
    {
		while (vCam.m_ScreenY > .5f)
        {
			yield return new WaitForFixedUpdate();
			vCam.m_ScreenY -= .005f;
		}
	}

	/// <summary>
	/// Sets the current player input.
	/// </summary>
	/// <param name="input"></param>
    public void SetDirectionalInput(Vector2 input) {
		directionalInput = input;
	}

	/// <summary>
	/// Logic for the player pushing the jump button.
	/// </summary>
	public void OnJumpInputDown() {
		if (isHoldingObject)
			return;

		currentNumberOfJumps -= 1;

		if (wallSliding) {
			if (wallDirX == directionalInput.x) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if (controller.collisions.below || currentNumberOfJumps > 0) {
			if (controller.collisions.slidingDownMaxSlope) {
				if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			}
			else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	/// <summary>
	/// Interrupts current jump if the player lets go of the jump key before the jump reaches its maximum height
	/// </summary>
	public void OnJumpInputUp()
	{
		if (velocity.y > minJumpVelocity)
		{
			velocity.y = minJumpVelocity;
		}
	}

	/// <summary>
	/// Changes character velocities when smashing, and sets the smashing state to true, if possible.
	/// </summary>
	public void OnSmashInputDown()
    {
		if(!controller.collisions.below && elementController.getElement().Equals("Earth"))
        {
			velocity.x = 0f;
			isSmashing = true;
			GetComponent<PlayerInput>().hasControl = false;
			StartCoroutine(SmashAnimation());
        }
    }

	/// <summary>
	/// Rotates the player in place before smashing them into the ground.
	/// </summary>
	/// <returns></returns>
	IEnumerator SmashAnimation()
    {
		float currentRot = 0f;
		Vector3 currentPos = gameObject.transform.position;
		while (currentRot < 360)
		{
			this.gameObject.transform.Rotate(new Vector3(0, 0, 20f), Space.Self);
			gameObject.transform.position = currentPos;
			currentRot += 20f;
			yield return new WaitForFixedUpdate();
		}

		velocity.y = smashSpeed;
	}

	/// <summary>
	/// Allows the player to grab onto bashable objects (which are highlighted in the game world when nearby), and launch themselves in a direction dictated by the mouse position.
	/// </summary>
	public void OnSlingshotInputDown()
    {
		if (!GameObject.Find("Game Manager").GetComponent<GameMaster>().playerHasInstantiated || !elementControllerHasInstantiated)
			return;

		try
        {
			if (!elementController.getElement().Equals("Water"))
				return;
		} catch
        {
			Debug.LogWarning("PlayerInputs: elementController has not yet instantiated. This message may repeat multiple times - please ignore unless it continues past 100 times.");
			return;
        }
		

		Vector2 currentPos = transform.position;

		RaycastHit2D[] rays = Physics2D.CircleCastAll(transform.position, radius, Vector3.forward);
		foreach(RaycastHit2D ray in rays)
        {
			nearToBashableObj = false;

			if(ray.collider.tag == "Bashable")
            {
				nearToBashableObj = true;
				bashableObj = ray.collider.transform.gameObject;
				break;
            }
        }
		if(nearToBashableObj)
        {
			bashableObj.GetComponent<SpriteRenderer>().color = Color.yellow;
			if(Input.GetKey(KeyCode.Mouse1))
            {
				transform.position = currentPos;
				bashableObj.transform.localScale = new Vector2(0.6f, 0.6f);
				arrow.SetActive(true);
				arrow.transform.position = bashableObj.transform.position;
				isChoosingDir = true;
				velocity.x = 0;
				velocity.y = 0;
				isBashing = true;
            } else if(isChoosingDir && Input.GetKeyUp(KeyCode.Mouse1))
            {
				bashableObj.transform.localScale = new Vector2(0.4f, 0.4f);
				isChoosingDir = false;
				isBashing = false;
				isMidBash = true;
				arrow.SetActive(false);

				transform.position = bashableObj.transform.position;
				Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
				float angle = Mathf.Atan2(direction.y, direction.x);
				lastAngle = Mathf.Cos(angle);
				velocity.x = Mathf.Clamp(Mathf.Cos(angle) * bashPower, -bashPower, bashPower);
				velocity.y = Mathf.Clamp(Mathf.Sin(angle) * bashPower,-bashPower*.6f,bashPower*.6f);
			}
        } else if(bashableObj != null)
        {
			bashableObj.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

	/// <summary>
	/// Dashes the player in their current direction
	/// </summary>
	public void OnDashInputDown()
	{
		currentNumberOfDashes -= 1;

		if ((controller.collisions.below || currentNumberOfDashes > 0) && elementController.getElement().Equals("Air"))
        {
			velocity.x = dashSpeed * directionalInput.x;
		}
	}

	/// <summary>
	/// Cause the player to search for enemies directly in front of it, and trigger the damage method in any which are present
	/// </summary>
	public void Attack()
	{
		if (!elementController.getElement().Equals("Fire"))
			return;

		//animator.SetTrigger("Attack");
		for (int i = 0; i < controller.horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (controller.collisions.faceDir == -1) ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (controller.horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * controller.collisions.faceDir, 2, enemyLayers);

			Debug.DrawRay(rayOrigin, Vector2.right * controller.collisions.faceDir * 2, Color.red);

			if(hit)
            {
				hit.collider.GetComponent<DestroyableDoor>().damageDoor(40);
				return;
            }
		}
	}

	/// <summary>
	/// Draw the area the player can attack in
	/// </summary>
	private void OnDrawGizmosSelected()
	{
		if (attackPoint == null)
			return;
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}

	/// <summary>
	/// Sets the player to be pulling an object, used for other logic
	/// </summary>
	public void OnPullingDown()
    {
		if(controller.collisions.isHoldingObject)
        {
			controller.collisions.isPulling = true;
			isHoldingObject = true;
        }
    }

	/// <summary>
	/// Unsets the player from pulling an object, used for other logic
	/// </summary>
	public void OnPullingUp()
    {
		try
        {
			controller.collisions.isPulling = false;
			isHoldingObject = false;
		} catch
        {
			Debug.LogWarning("Warning: PlayerInputs has failed to initialise the player collisions struct isPulling variable. If this message repeats, please contact Matt.");
        }
		
	}

	/// <summary>
	/// Logic for determining the velocity while the player is wall-sliding
	/// </summary>
	void HandleWallSliding()
	{
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0 && !controller.collisions.isHoldingObject) {
			wallSliding = true;
			currentNumberOfJumps = numberOfJumps + 1;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}
		}
	}

	/// <summary>
	/// Calculates the velocity of the player based off the player input
	/// </summary>
	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}
}
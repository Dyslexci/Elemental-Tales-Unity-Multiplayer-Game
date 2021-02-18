﻿using UnityEngine;
using System.Collections;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
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

	/// <summary>
	/// Initialises the player components and physical parameters.
	/// </summary>
	void Start()
	{
		currentNumberOfJumps = numberOfJumps;
		currentNumberOfDashes = numberOfDashes;
		controller = GetComponent<CharacterControllerRaycast>();
		elementController = GetComponent<ElementController>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	/// <summary>
	/// Calculates and sends off the current desired/expected velocity, taking into account obstacles, gravity and player input.
	/// </summary>
	void FixedUpdate()
    {
		if (controller.collisions.below)
        {
			currentNumberOfJumps = numberOfJumps;
			currentNumberOfDashes = numberOfDashes;
		}
            

		CalculateVelocity();
        HandleWallSliding();

        if (controller.collisions.below && directionalInput.y == -1)
        {
            controller.Move(velocity * Time.deltaTime * 0.5f, directionalInput);
            return;
        }

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
		controller.collisions.isPulling = false;
		isHoldingObject = false;
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
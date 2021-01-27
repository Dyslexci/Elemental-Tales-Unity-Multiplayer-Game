using UnityEngine;
using System.Collections;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Takes input from the player and converts to logic which can be used by controller classes.
 */

[RequireComponent(typeof(PlayerInputs))]
public class PlayerInput : MonoBehaviour
{
	PlayerInputs player;

	// Initialises the player inputs script
	void Start()
	{
		player = GetComponent<PlayerInputs>();
	}

	// Accepts player input and sorts jump logic
	void Update()
	{
		Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		player.SetDirectionalInput(directionalInput);

		if (Input.GetKeyDown(KeyCode.Space) && !(directionalInput.y == -1 && player.controller.collisions.isOnPermeable))
		{
			player.OnJumpInputDown();
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			player.OnJumpInputUp();
		}
	}
}
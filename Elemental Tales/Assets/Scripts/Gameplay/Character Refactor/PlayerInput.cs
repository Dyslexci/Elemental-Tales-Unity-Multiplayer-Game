﻿using UnityEngine;
using System.Collections;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Takes input from the player and converts to logic which can be used by controller classes.
 */

[RequireComponent(typeof(PlayerInputs))]
public class PlayerInput : MonoBehaviourPun
{
	PlayerInputs player;
	public bool hasControl;
	GameMaster gameMaster;

	// Initialises the player inputs script
	void Start()
	{
		player = GetComponent<PlayerInputs>();
		gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
		hasControl = true;
	}

	// Accepts player input and sorts jump logic
	void Update()
	{
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			return;

		if (hasControl)
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
			if (Input.GetAxisRaw("PushPull") == 1)
			{
				player.OnPullingDown();
			}
			else
			{
				if(player != null)
                {
					player.OnPullingUp();
				}
			}
			if(Input.GetButtonDown("Pause"))
            {
				gameMaster.PauseGame();
            }
		} else
        {
			Vector2 directionalInput = new Vector2(0, 0);
			player.SetDirectionalInput(directionalInput);
		}
	}
}
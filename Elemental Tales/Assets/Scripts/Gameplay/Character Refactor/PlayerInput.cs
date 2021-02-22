using UnityEngine;
using System.Collections;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.1.0
 *    
 *    Takes input from the player and converts to logic which can be used by controller classes.
 */

[RequireComponent(typeof(PlayerInputs))]
public class PlayerInput : MonoBehaviourPun
{
	PlayerInputs player;
	public bool hasControl;
	GameMaster gameMaster;

	public bool jumpKeyDown;
	public bool pushKeyDown;
	public bool pauseKeyDown;
	public bool interactKeyDown;

	[SerializeField] private float attackRate = 2f;
	private float nextAttackTime = 0f;

	/// <summary>
	/// Initialises the player inputs script.
	/// </summary>
	void Start()
	{
		player = GetComponent<PlayerInputs>();
		gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
		hasControl = true;
	}

	/// <summary>
	/// Accepts player input and sorts jump logic
	/// </summary>
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
				jumpKeyDown = true;
			}
			if (Input.GetKeyUp(KeyCode.Space))
			{
				player.OnJumpInputUp();
				jumpKeyDown = false;
			}
			if (Input.GetAxisRaw("PushPull") == 1)
			{
				player.OnPullingDown();
				pushKeyDown = true;
			}
			else
			{
				if(player != null)
                {
					player.OnPullingUp();
				}
				pushKeyDown = false;
			}
			if(Input.GetButtonDown("Pause"))
            {
				gameMaster.PauseGame();
				pauseKeyDown = true;
            } else
            {
				pauseKeyDown = false;
            }
			if(Input.GetButtonDown("Interact"))
            {
				interactKeyDown = true;
            } else
            {
				interactKeyDown = false;
            }
			if(Input.GetKeyDown(KeyCode.LeftControl))
            {
				player.OnDashInputDown();
            }
			if(Input.GetKeyDown(KeyCode.S))
            {
				player.OnSmashInputDown();
            }
			if(Input.GetKey(KeyCode.Mouse1))
            {
				player.OnSlingshotInputDown();
            }
			if(Input.GetKeyDown(KeyCode.W) && player.controller.collisions.below)
            {
				player.PanCamUpKeyDown();
            }
			if(Input.GetKeyUp(KeyCode.W))
            {
				player.PanCamUpKeyUp();
            }
			if(Input.GetKeyDown(KeyCode.S) && player.controller.collisions.below)
            {
				player.PanCamDownKeyDown();
            }
			if(Input.GetKeyUp(KeyCode.S))
            {
				player.PanCamDownKeyUp();
            }
			if(Input.GetKeyDown(KeyCode.Mouse0))
            {
				if (Time.time >= nextAttackTime)
				{
					player.Attack();
					nextAttackTime = Time.time + 1f / attackRate;
				}
			}
		} else
        {
			Vector2 directionalInput = new Vector2(0, 0);
			player.SetDirectionalInput(directionalInput);
		}
	}
}
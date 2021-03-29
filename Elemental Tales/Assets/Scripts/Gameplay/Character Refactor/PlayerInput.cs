using Photon.Pun;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 2.1.0
 *
 *    Takes input from the player and converts to logic which can be used by controller classes.
 */

[RequireComponent(typeof(PlayerInputs))]
public class PlayerInput : MonoBehaviourPun
{
    private PlayerInputs player;
    public bool hasControl;
    private GameMaster gameMaster;

    public bool jumpKeyDown;
    public bool pushKeyDown;
    public bool pauseKeyDown;
    public bool interactKeyDown;

    [SerializeField] private float attackRate = 2f;
    private float nextAttackTime = 0f;

    private float dashRate = 3.5f;
    private float nextDashTime = 0f;

    /// <summary>
    /// Initialises the player inputs script.
    /// </summary>
    private void Start()
    {
        player = GetComponent<PlayerInputs>();
        gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
        hasControl = true;
    }

    /// <summary>
    /// Accepts player input and sorts jump logic
    /// </summary>
    private void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        if (Input.GetButtonDown("Pause"))
        {
            gameMaster.PauseGame();
            pauseKeyDown = true;
        }
        else
        {
            pauseKeyDown = false;
        }

        if (hasControl)
        {
            PlayerInputControls();
        }
        else
        {
            Vector2 directionalInput = new Vector2(0, 0);
            player.SetDirectionalInput(directionalInput);
        }
    }

    private void PlayerInputControls()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && !(directionalInput.y == -1 && player.controller.collisions.isOnPermeable))
        {
            player.OnJumpInputDown();
            jumpKeyDown = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W))
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
            if (player != null)
            {
                player.OnPullingUp();
            }
            pushKeyDown = false;
        }
        if (Input.GetButtonDown("Interact"))
        {
            interactKeyDown = true;
        }
        else
        {
            interactKeyDown = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Time.time >= nextDashTime)
            {
                player.OnDashInputDown();
                nextDashTime = Time.time + 1f / dashRate;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            player.OnSmashInputDown();
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            player.OnSlingshotInputDown();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (Time.time >= nextAttackTime)
            {
                player.Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }
}
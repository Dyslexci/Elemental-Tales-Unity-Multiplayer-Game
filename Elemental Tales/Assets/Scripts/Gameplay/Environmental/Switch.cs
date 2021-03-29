using Photon.Pun;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 2.0.2
 *
 *    Manages players interacting with physical switch objects in the world.
 */

public class Switch : CheckPresentController
{
    private GameMaster gameMaster;
    private UIHintController hintController;

    [SerializeField] private Sprite crankDown;
    [SerializeField] private Sprite crankUp;
    [SerializeField] private Transform pos;
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private LayerMask layer;
    private bool isOn = false;
    public bool pressedSuccessfully = false;
    private bool playerPresent = false;
    private bool otherPlayerPresent;
    private bool playerWasPresent;

    /// <summary>
    /// Initialises switch values.
    /// </summary>
    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = crankDown;
        gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
        hintController = GameObject.Find("Game Manager").GetComponent<UIHintController>();
    }

    /// <summary>
    /// Checks for player presence and whether the player is pulling the switch. Triggers an RPC if the player is interacting with the switch.
    /// </summary>
    private void FixedUpdate()
    {
        if (pressedSuccessfully)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = crankUp;
            return;
        }
        playerWasPresent = playerPresent;
        playerPresent = false;
        otherPlayerPresent = false;
        CheckPresentCircle();
        CheckForPlayerInteraction();
    }

    private void CheckForPlayerInteraction()
    {
        if (playerPresent && Input.GetButton("Interact"))
        {
            if (isOn == true)
                return;
            photonView.RPC("setLeverOn", RpcTarget.AllBuffered);
            gameMaster.switchPull.Play(0);
        }
        else if (playerPresent && !otherPlayerPresent)
        {
            if (isOn == false)
                return;
            photonView.RPC("setLeverOff", RpcTarget.AllBuffered);
            gameMaster.switchPull.Play(0);
        }
    }

    /// <summary>
    /// Refactored by Adnan
    /// Checks for the players presence based off Physics2D collider circles and displays the hint if the player has entered the switch collider.
    /// </summary>
    public void CheckPresentCircle()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.position, radius, layer);

        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Player")
            {
                if (c.gameObject.GetPhotonView().IsMine)
                {
                    if (hintController.isDisplayingHint == false && playerWasPresent == false)
                    {
                        hintController.isDisplayingHint = true;
                        gameMaster.hintSound.Play(0);
                        hintController.StartHintDisplay("<color=#ffffff>Hold E to <color=#ffeb04>grab and switch <color=#ffffff>levers!", 2);
                    }
                    playerPresent = true;
                    return;
                }
                if (!c.gameObject.GetPhotonView().IsMine)
                {
                    otherPlayerPresent = true;
                }
            }
        }
    }

    /// <summary>
    /// Draws a cisual representation of the switch collider in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(pos.position, radius);
    }

    /// <summary>
    /// Returns the current state of the lever.
    /// </summary>
    /// <returns></returns>
    public bool getLeverState()
    {
        return isOn;
    }

    /// <summary>
    /// Sets the state of the lever to successfully pressed once the final condition of the interaction is filled; e.g. the door has opened.
    /// </summary>
    public void setPressedSuccessfully()
    {
        pressedSuccessfully = true;
        isOn = false;
    }

    /// <summary>
    /// Sends an RPC to other player triggering the lever to its on state.
    /// </summary>
    [PunRPC]
    private void setLeverOn()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = crankUp;
        isOn = true;
    }

    /// <summary>
    /// Sends an RPC to the other player triggering the lever to its off state.
    /// </summary>
    [PunRPC]
    private void setLeverOff()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = crankDown;
        isOn = false;
    }
}
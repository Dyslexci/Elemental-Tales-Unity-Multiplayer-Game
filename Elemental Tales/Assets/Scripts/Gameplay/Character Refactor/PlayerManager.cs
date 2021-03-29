using Photon.Pun;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 2.3.0
 *    @version 1.0.0
 *
 *    Manages various player processes which don't belong in other player scripts.
 */

public class PlayerManager : MonoBehaviourPun
{
    public GameObject player;
    public SpriteRenderer playerSprite;

    private void Start()
    {
        player = gameObject;

        if (photonView.IsMine)
            photonView.RPC("SetPlayerColour", RpcTarget.AllBuffered, GlobalVariableManager.PlayerColour);
    }

    /// <summary>
    /// Calls the RPC to despawn the player.
    /// </summary>
    public void DespawnPlayer()
    {
        photonView.RPC("DespawnPlayerModel", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Calls the RPC to respawn the player.
    /// </summary>
    public void RespawnPlayer()
    {
        photonView.RPC("RespawnPlayerModel", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Sets the player object to inactive for all players.
    /// </summary>
    [PunRPC]
    private void DespawnPlayerModel()
    {
        player.SetActive(false);
    }

    /// <summary>
    /// Sets the player object to active for all players.
    /// </summary>
    [PunRPC]
    private void RespawnPlayerModel()
    {
        player.SetActive(true);
    }

    /// <summary>
    /// Sets the player colour to the chosen colour for each player.
    /// </summary>
    /// <param name="hex"></param>
    [PunRPC]
    private void SetPlayerColour(string hex)
    {
        if (hex.Equals("FFFFFF"))
        {
            return;
        }
        if (hex.Equals("FFCD81"))
        {
            playerSprite.color = new Color(1, 0.801076f, 0.504717f);
            return;
        }
        if (hex.Equals("B581FF"))
        {
            playerSprite.color = new Color(0.7103899f, 0.5058824f, 1);
            return;
        }
        if (hex.Equals("81FAFF"))
        {
            playerSprite.color = new Color(0.5058824f, 0.9781956f, 1);
            return;
        }
    }
}
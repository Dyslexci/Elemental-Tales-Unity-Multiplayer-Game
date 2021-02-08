using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
 *    
 *    Ends the game when the player enters the endgame region.
 */
public class endGame : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Loads the main menu when the player has successfully left the room.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// When the player has entered the collider region, disconnect them from the room and load the EndgameScene scene.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetPhotonView().IsMine == false && PhotonNetwork.IsConnected == true)
                return;

            PhotonNetwork.LoadLevel("EndgameScene");
        }
    }
}

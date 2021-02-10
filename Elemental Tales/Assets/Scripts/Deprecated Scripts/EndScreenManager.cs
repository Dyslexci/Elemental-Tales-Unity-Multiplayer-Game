using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.1
 *    
 *    Causes the player to leave the room, only for use in the post-level screen.
 */
public class EndScreenManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// When the player leaves the room, load the main menu.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Triggers the player to leave the room.
    /// </summary>
    public void exitGame()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}

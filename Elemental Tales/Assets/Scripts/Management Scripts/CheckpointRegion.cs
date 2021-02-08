using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
 *    
 *    Provides checkpoints for the player to reset to on death.
 */

public class CheckpointRegion : MonoBehaviourPun
{
    /// <summary>
    /// When a player enters the checkpoint collider, update their latest checkpoint to this checkpoint.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Entered checkpoint");
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetPhotonView().IsMine == false && PhotonNetwork.IsConnected == true)
                return;

            GameObject.Find("Game Manager").GetComponent<GameMaster>().setCheckpoint(this.gameObject.GetComponentsInChildren<Transform>()[0]);
            Debug.Log("Checkpoint has been set to " + this.gameObject.name);
        }
    }
}

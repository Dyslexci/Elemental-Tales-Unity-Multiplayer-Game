using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 2.0.0
 *    @version 1.0.0
 *    
 *    Adds mana to both players and deletes the associated collectible object when player object collides.
 */

public class ManaCollector : MonoBehaviourPun
{
    private double hitLast = 0;
    private double hitDelay = 0.2;

    /// <summary>
    /// On player object collision, add the collectible to the collectible counter and remove the collectible object from the game.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetPhotonView().IsMine == false && PhotonNetwork.IsConnected == true)
                return;
            if (Time.time - hitLast < hitDelay)
                return;

            print("Playing gem sound");
            GameObject.Find("Game Manager").GetComponent<GameMaster>().manaCollector.Play(0);
            //print("Collected");
            photonView.RPC("AddHealth", RpcTarget.AllBuffered);
            Debug.Log("PUN: RPC has been sent to delete the collectible and add to the counter.");
            hitLast = Time.time;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Sends an RPC to delete the collectible and add a collectible to the counter for both players.
    /// </summary>
    [PunRPC] private void AddHealth()
    {
        Debug.Log("PUN: deleteGem() has been called via the RPC.");
        FindObjectOfType<GameMaster>().getPlayer().GetComponent<StatController>().IncreaseMaxMana();
        this.gameObject.SetActive(false);
    }
}

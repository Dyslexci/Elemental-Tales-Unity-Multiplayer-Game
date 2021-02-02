using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class GemCollector : MonoBehaviourPun
{
    private double hitLast = 0;
    private double hitDelay = 0.2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetPhotonView().IsMine == false && PhotonNetwork.IsConnected == true)
                return;
            if (Time.time - hitLast < hitDelay)
                return;

            GameObject.Find("Game Manager").GetComponent<GameMaster>().collectGem1Sound.Play(0);
            //print("Collected");
            photonView.RPC("deleteGem", RpcTarget.AllBuffered);
            Debug.Log("PUN: RPC has been sent to delete the collectible and add to the counter.");
            hitLast = Time.time;
            gameObject.SetActive(false);
        }
    }

    [PunRPC] private void deleteGem()
    {
        Debug.Log("PUN: deleteGem() has been called via the RPC.");
        FindObjectOfType<GameMaster>().addCollectible1();
        this.gameObject.SetActive(false);
    }
}

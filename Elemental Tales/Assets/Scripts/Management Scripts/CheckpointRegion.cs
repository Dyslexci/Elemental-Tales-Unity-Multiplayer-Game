using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class CheckpointRegion : MonoBehaviourPun
{

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

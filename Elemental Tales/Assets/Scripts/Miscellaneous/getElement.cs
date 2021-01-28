using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class getElement : MonoBehaviourPun
{
    [SerializeField] private string heldElement;
    ElementController elementController;

    private void Start()
    {
        elementController = GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<ElementController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetPhotonView().IsMine == false && PhotonNetwork.IsConnected == true)
                return;

            elementController.addElement(heldElement);
            photonView.RPC("addElement", RpcTarget.AllBuffered);
            Debug.Log("PUN: RPC has been sent to add the element to all players.");
            gameObject.SetActive(false);
        }
    }

    [PunRPC] private void addElement()
    {
        Debug.Log("PUN: addElement() has been called, adding the element to the player.");
        elementController.addElement(heldElement);
        gameObject.SetActive(false);
    }
}

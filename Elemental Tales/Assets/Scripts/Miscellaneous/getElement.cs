using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 0.1.0
 *    @version 1.0.0
 *    
 *    Adds new elements to the player objects.
 */
public class getElement : MonoBehaviourPun
{
    [SerializeField] private string heldElement;
    ElementController elementController;

    /// <summary>
    /// Initialises the player object.
    /// </summary>
    private void Update()
    {
        if(elementController == null)
            InitialisePlayer();
    }

    /// <summary>
    /// Initialises the element controller on the local player object.
    /// </summary>
    void InitialisePlayer()
    {
        elementController = GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<ElementController>();
        print("getElement: InitialisePlayer() has been called, initialising player element controller has succeeded");
    }

    /// <summary>
    /// Adds the element to the local player and triggers the RPC to do the same for the other players.
    /// </summary>
    /// <param name="collision"></param>
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

    /// <summary>
    /// RPC to add the element to all players in the game and remove the element holder.
    /// </summary>
    [PunRPC] private void addElement()
    {
        Debug.Log("PUN: addElement() has been called, adding the element to the player.");
        elementController.addElement(heldElement);
        gameObject.SetActive(false);
    }
}

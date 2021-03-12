using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 0.1.0
 *    @version 2.0.0
 *    
 *    Adds new elements to the player objects.
 */
public class getElement : MonoBehaviourPun
{
    [SerializeField] private string heldElement;
    ElementController elementController;
    NPCBehaviourTemp npcManager;

    private void Start()
    {
        npcManager = GetComponent<NPCBehaviourTemp>();
    }

    /// <summary>
    /// Initialises the player object.
    /// </summary>
    private void FixedUpdate()
    {
        if(elementController == null)
            InitialisePlayer();

        if(npcManager.hasTalked)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// Initialises the element controller on the local player object.
    /// </summary>
    void InitialisePlayer()
    {
        try
        {
            elementController = GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<ElementController>();
            print("getElement: InitialisePlayer() has been called, initialising player element controller has succeeded");
        } catch
        {
            Debug.LogWarning("Warning: getElement class has not correctly initialised elementController variable. This is expected to happen once or twice, but if this message repeats multiple times" +
                ", please contact Matt.");
        }
        
        
    }

    /// <summary>
    /// Adds the element to the local player and triggers the RPC to do the same for the other players.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (npcManager.isTalking)
            return;

        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetPhotonView().IsMine == false && PhotonNetwork.IsConnected == true)
                return;

            elementController.addElement(heldElement);
            photonView.RPC("addElement", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// RPC to add the element to all players in the game and remove the element holder.
    /// </summary>
    [PunRPC] private void addElement()
    {
        Debug.Log("PUN: addElement() has been called, adding the element to the player.");
        elementController.addElement(heldElement);
        //FindObjectOfType<DialogueManager>().StartDialogue(GetComponent<DialogueTrigger>().dialogue, npcManager, GetComponent<DialogueTrigger>().quest);
        StartCoroutine(TriggerDialogue());
    }

    /// <summary>
    /// Triggers the dialogue associated with this object.
    /// </summary>
    /// <returns></returns>
    IEnumerator TriggerDialogue()
    {

        GameMaster gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
        gameMaster.getPlayer().GetComponent<PlayerInput>().hasControl = false;

        gameMaster.elementalGetStart.Play(0);
        while(gameMaster.elementalGetStart.isPlaying)
        {
            yield return new WaitForFixedUpdate();
        }
        gameMaster.elementDialogueStart.Play(0);
        FindObjectOfType<DialogueManager>().StartDialogue(GetComponent<DialogueTrigger>().dialogue, npcManager, GetComponent<DialogueTrigger>().quest);
        npcManager.isTalking = true;
        gameObject.SetActive(false);
    }
}

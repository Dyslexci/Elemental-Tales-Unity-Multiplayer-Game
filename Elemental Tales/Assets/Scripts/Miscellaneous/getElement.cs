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
    public NPCBehaviourTemp npcManager;
    public NPCBehaviourTemp npcManager2;
    public NPCBehaviourTemp npcManager3;
    public NPCBehaviourTemp npcManager4;

    public DialogueTrigger dialogue1;
    public DialogueTrigger dialogue2;
    public DialogueTrigger dialogue3;
    public DialogueTrigger dialogue4;

    /// <summary>
    /// Initialises the player object.
    /// </summary>
    private void FixedUpdate()
    {
        if(elementController == null)
            InitialisePlayer();

        if(npcManager.hasTalked || npcManager2.hasTalked || npcManager3.hasTalked || npcManager4.hasTalked)
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

        gameMaster.elementDialogueStart.Play(0);
        while(gameMaster.elementDialogueStart.isPlaying)
        {
            yield return new WaitForFixedUpdate();
        }

        if(gameMaster.mapStage == 0)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue1.dialogue, npcManager, dialogue1.quest);
            npcManager.isTalking = true;
        } else if(gameMaster.mapStage == 1)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue2.dialogue, npcManager2, dialogue2.quest);
            npcManager2.isTalking = true;
        } else if(gameMaster.mapStage == 2)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue3.dialogue, npcManager3, dialogue3.quest);
            npcManager3.isTalking = true;
        } else if(gameMaster.mapStage == 3)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue4.dialogue, npcManager4, dialogue4.quest);
            npcManager4.isTalking = true;
        }        
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.1.0
 *    
 *    This script drives behaviour for interactible NPCs placed in the gameworld, specifically allowing the player to interact on keypress.
 */


public class NPCBehaviourTemp : MonoBehaviour
{
    [SerializeField] private Quest quest;

    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private DialogueTrigger dialogueTriggerHasTalked;

    public bool isNPC = true;

    bool playerInArea = false;
    public bool hasTalked = false;

    TMP_Text hintText;
    GameObject hintHolder;
    CanvasGroup panel;
    Image hintImage;
    bool isDisplayingHint;

    GameObject player;

    public bool isTalking;

    /// <summary>
    /// Initialises the HUD elements
    /// </summary>
    private void Start()
    {
        if (!isNPC)
            return;

        hintText = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintText();
        hintHolder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().GetHintHolder();
        hintImage = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintContainer();
        panel = hintHolder.GetComponentInChildren<CanvasGroup>();
    }

    /// <summary>
    /// Triggers associated NPC dialogue when the player is interacting with the NPC.
    /// </summary>
    private void FixedUpdate()
    {
        if (!isNPC)
            return;

        if (Input.GetButtonDown("Interact") && playerInArea && !hasTalked && !isTalking)
        {
            isTalking = true;
            dialogueTrigger.TriggerDialogue();
            hasTalked = true;
            print("Is speaking for the first time");
        } else if (Input.GetButtonDown("Interact") && playerInArea && hasTalked && !isTalking)
        {
            isTalking = true;
            dialogueTriggerHasTalked.TriggerDialogue();
            print("Is speaking for the second time");
        }
    }

    /// <summary>
    /// Checks for player presence next to the NPC.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isNPC)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            playerInArea = true;
            player = collision.gameObject;
            if(!isDisplayingHint)
            {
                GameObject.Find("Game Manager").GetComponent<GameMaster>().hintSound.Play(0);
                StartCoroutine(WaitHideHint());
            }
        }
    }

    /// <summary>
    /// Checks for player leaving the NPC area.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isNPC)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            playerInArea = false;
            player = null;
        }
    }

    /// <summary>
    /// Coroutine displaying the hint to the player.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitHideHint()
    {
        hintHolder.SetActive(true);
        StartCoroutine("JumpInHintHolder");
        hintText.text = "<color=#ffffff>Press E to <color=#ffeb04> talk <color=#ffffff>to <color=#ffeb04>" + GetComponent<DialogueTrigger>().dialogue.name + "<color=#ffffff>!";
        yield return new WaitForSeconds(2);
        StartCoroutine("FadeHintHolder");
    }

    /// <summary>
    /// Coroutine making the hint jump into place.
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpInHintHolder()
    {
        hintImage.transform.localScale = new Vector3(5, 5, 5);

        while (hintImage.transform.localScale.x > 1)
        {
            yield return new WaitForFixedUpdate();
            hintImage.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        }
        hintImage.transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Coroutine making the hint fade out after it has been on screen enough time.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeHintHolder()
    {
        while (panel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            panel.alpha -= 0.05f;
        }

        hintHolder.SetActive(false);
        panel.alpha = 1;
        isDisplayingHint = false;
    }
}

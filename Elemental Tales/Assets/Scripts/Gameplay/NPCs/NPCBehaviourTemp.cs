﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    This script drives behaviour for interactible NPCs placed in the gameworld, specifically allowing the player to interact on keypress.
 */


public class NPCBehaviourTemp : MonoBehaviour
{
    [SerializeField] private Quest quest;

    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private DialogueTrigger dialogueTriggerHasTalked;

    private Boolean playerInArea = false;
    private Boolean hasTalked = false;

    /// <summary>
    /// Triggers associated NPC dialogue when the player is interacting with the NPC.
    /// </summary>
    private void Update()
    {
        if (Input.GetButtonDown("Interact") && playerInArea && !hasTalked)
        {
            hasTalked = true;
            dialogueTrigger.TriggerDialogue();
            print(hasTalked);
        } else if (Input.GetButtonDown("Interact") && playerInArea && hasTalked)
        {
            dialogueTriggerHasTalked.TriggerDialogue();
        }
    }

    /// <summary>
    /// Checks for player presence next to the NPC.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInArea = true;
        }
    }

    /// <summary>
    /// Checks for player leaving the NPC area.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInArea = false;
        }
    }
}

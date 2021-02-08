using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Triggers dialogue when called.
 */

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Quest quest;

    /// <summary>
    /// Triggers the dialogue with the given dialogue and quest parameters.
    /// </summary>
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, quest);
    }
}

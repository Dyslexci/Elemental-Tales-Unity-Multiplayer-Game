using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.0.0
 *
 *    Triggers dialogue when called.
 */

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Quest quest;
    public NPCBehaviourTemp NPCObject;

    private void Start()
    {
        NPCObject = GetComponent<NPCBehaviourTemp>();
    }

    /// <summary>
    /// Triggers the dialogue with the given dialogue and quest parameters.
    /// </summary>
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, NPCObject, quest);
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.2.1
 *    
 *    This script controls dialogue behaviour for the entire scene, implementing various different dialogue scripts.
 */

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    [SerializeField] public GameObject canvasObect;
    private Quest newQuest;
    private bool inDialogue = false;
    private NPCBehaviourTemp NPCObject;

    public Animator animator;

    public Queue<string> sentences;

    public AudioSource[] typeLetter;
    public AudioSource openDialogueBox;
    public AudioSource closeDialogueBox;
    public AudioSource endSentence;

    /// <summary>
    /// Sets up a Queue for the sentences.
    /// </summary>
    private void Start()
    {
        sentences = new Queue<string>();
    }

    /// <summary>
    /// Displays the next sentence while the F key is held down.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inDialogue == true)
        {
            DisplayNextSentence();
        }

    }

    /// <summary>
    /// Starts the dialogue, displaying the correct canvas and writing on the initial line.
    /// </summary>
    /// <param name="dialogue"></param>
    /// <param name="quest"></param>
    public void StartDialogue(Dialogue dialogue, NPCBehaviourTemp _NPCObject, Quest quest = null)
    {
        GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<PlayerInput>().hasControl = false;
        inDialogue = true;
        NPCObject = _NPCObject;
        NPCObject.isTalking = true;
        if (!(quest == null))
            newQuest = quest;
        canvasObect.SetActive(true);
        animator.SetBool("isOpen", true);
        openDialogueBox.Play(0);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    /// <summary>
    /// Displays the next sentence in the queue of sentences.
    /// </summary>
    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    /// <summary>
    /// Types out each letter of the sentence individually.
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    private IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            if(letter.Equals('%'))
            {
                dialogueText.text += "<color=#ffeb04>";
                continue;
            } else if(letter.Equals('$'))
            {
                dialogueText.text += "<color=#ffffff>";
                continue;
            }
            dialogueText.text += letter;
            typeLetter[Random.Range(0, typeLetter.Length)].Play(0);
            yield return new WaitForFixedUpdate();
        }
        endSentence.Play(0);
    }

    /// <summary>
    /// Ends the dialogue and currently adds a quest to the player object.
    /// </summary>
    private void EndDialogue()
    {
        NPCObject.isTalking = false;
        NPCObject.hasTalked = true;
        inDialogue = false;
        if(newQuest.getQuestable())
        {
            acceptQuest();
        }
        animator.SetBool("isOpen", false);
        closeDialogueBox.Play(0);
        canvasObect.SetActive(false);
        GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<PlayerInput>().hasControl = true;
    }

    /// <summary>
    /// Adds the quest to the player object QuestManager.
    /// </summary>
    private void acceptQuest()
    {
        if (newQuest == null)
            return;
        FindObjectOfType<QuestManager>().addQuest(newQuest);
        newQuest.setActive();
    }
}

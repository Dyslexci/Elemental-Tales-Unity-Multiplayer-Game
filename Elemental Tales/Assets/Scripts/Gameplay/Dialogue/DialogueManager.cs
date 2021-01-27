﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
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

    public Animator animator;

    public Queue<string> sentences;
    private void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inDialogue == true)
        {
            DisplayNextSentence();
        }

    }

    public void StartDialogue(Dialogue dialogue, Quest quest = null)
    {
        inDialogue = true;
        if(!(quest == null))
            newQuest = quest;
        canvasObect.SetActive(true);
        animator.SetBool("isOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        Debug.Log("Next sentence");
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return 1;
        }
    }

    private void EndDialogue()
    {
        inDialogue = false;
        if(newQuest.getQuestable())
        {
            Debug.Log("Quest added");
            acceptQuest();
        }
        animator.SetBool("isOpen", false);
        canvasObect.SetActive(false);
    }

    private void acceptQuest()
    {
        if (newQuest == null)
            return;
        FindObjectOfType<QuestManager>().addQuest(newQuest);
        newQuest.setActive();
    }
}
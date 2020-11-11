using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public List<Quest> quests;
    [SerializeField] private Image tasksContainer;
    [SerializeField] private TMP_Text quest1Title;
    [SerializeField] private TMP_Text quest1Desc;
    private int noQuests;

    private void Update()
    {
        if(quests.Count == 0)
        {
            animator.SetBool("isOpen", false);
        } else
        {
            animator.SetBool("isOpen", true);
            quest1Title.text = quests[0].getTitle();
            quest1Desc.text = quests[0].getDesc();
        }
    }

    public void addQuest(Quest quest)
    {
        quests.Add(quest);
    }
}

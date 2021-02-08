using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
 *    
 *    Manages the quests panel on the HUD, and stores active quests.
 */

public class QuestManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public List<Quest> quests;
    [SerializeField] private Image tasksContainer;
    [SerializeField] private TMP_Text quest1Title;
    [SerializeField] private TMP_Text quest1Desc;
    private int noQuests;

    /// <summary>
    /// Updates the HUD with quest information.
    /// </summary>
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

    /// <summary>
    /// Adds the parameter quest to the list of quests held by the manager.
    /// </summary>
    /// <param name="quest"></param>
    public void addQuest(Quest quest)
    {
        quests.Add(quest);
    }
}

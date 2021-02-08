﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.0.0
 *    
 *    Provides variables for the quest.
 */

[System.Serializable]
public class Quest
{
    [SerializeField] private bool isActive;
    [SerializeField] private bool isQuestable;

    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private int currencyReward;

    public bool getQuestable()
    {
        return isQuestable;
    }

    public void setActive()
    {
        isActive = true;
    }

    public string getTitle()
    {
        return title;
    }

    public string getDesc()
    {
        return description;
    }
}

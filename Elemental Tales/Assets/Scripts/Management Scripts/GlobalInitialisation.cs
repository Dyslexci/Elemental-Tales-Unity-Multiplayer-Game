using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInitialisation : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("level1Stage"))
        {
            PlayerPrefs.SetInt("level1Stage", 0);
        }
        if(!PlayerPrefs.HasKey("level1BestTime"))
        {
            PlayerPrefs.SetFloat("level1BestTime", 0);
        }

        GlobalVariableManager.Level1Stage = PlayerPrefs.GetInt("level1Stage");
        GlobalVariableManager.Level1BestTime = PlayerPrefs.GetFloat("level1BestTime");
        GlobalVariableManager.Level1TimeHasChanged = false;

        Debug.Log("Current level 1 stage: " + GlobalVariableManager.Level1Stage);
        Debug.Log("Current level 1 best time: " + GlobalVariableManager.Level1BestTime);
    }
}

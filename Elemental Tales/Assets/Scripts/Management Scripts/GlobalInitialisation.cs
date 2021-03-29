using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 2.1.0
 *    @version 1.0.0
 *
 *    Sets up playerpref values and global variable manager.
 */

public class GlobalInitialisation : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("level1Stage"))
        {
            PlayerPrefs.SetInt("level1Stage", 0);
        }
        if (!PlayerPrefs.HasKey("level1BestTime"))
        {
            PlayerPrefs.SetFloat("level1BestTime", 0);
        }

        GlobalVariableManager.Level1Stage = PlayerPrefs.GetInt("level1Stage");
        GlobalVariableManager.Level1BestTime = PlayerPrefs.GetFloat("level1BestTime");
        GlobalVariableManager.Level1TimeHasChanged = false;
    }
}
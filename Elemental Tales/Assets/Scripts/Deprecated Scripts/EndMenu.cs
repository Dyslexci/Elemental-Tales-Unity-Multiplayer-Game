using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Exits the application to desktop - only for use in an exit to desktop button.
 */
public class EndMenu : MonoBehaviour
{
    /// <summary>
    /// Quits the application.
    /// </summary>
    public void quitGame()
    {
        Debug.Log("Quit successful");
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/** 
 *    @author Himesh
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    For Himesh to fill out.
 */

public class Level : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel(int x)
    {
        SceneManager.LoadScene(x);
    }
}

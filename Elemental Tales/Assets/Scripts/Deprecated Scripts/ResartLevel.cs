using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Deprecated

public class ResartLevel : MonoBehaviour
{
    /// <summary>
    /// Reloads the active scene.
    /// </summary>
    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

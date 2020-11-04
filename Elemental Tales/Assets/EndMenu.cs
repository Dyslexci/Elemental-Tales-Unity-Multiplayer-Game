using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public void quitGame()
    {
        Debug.Log("Quit successful");
        Application.Quit();
    }
}

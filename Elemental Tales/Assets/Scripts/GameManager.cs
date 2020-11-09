using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneMangement;
public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel(int x)
    {
        SceneManager.LoadScene(x);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

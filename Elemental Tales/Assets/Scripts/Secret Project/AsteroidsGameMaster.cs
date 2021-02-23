using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AsteroidsGameMaster : MonoBehaviour
{
    int score = 0;
    TimerController timer;
    public TMP_Text scoreText;
    public float backgroundOffset = 102f;
    public float backgroundTriggerX = 54.5f;
    public Transform startBackground;

    public GameObject backgroundPrefab;
    GameObject lastCreatedBackground;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score:   0";
        timer = GetComponent<TimerController>();
        timer.BeginTimer();
        Instantiate(backgroundPrefab, new Vector2(backgroundTriggerX, 0.5f), Quaternion.identity);
        Instantiate(backgroundPrefab, new Vector2(backgroundTriggerX - backgroundOffset, 0.5f), Quaternion.identity);
        lastCreatedBackground = Instantiate(backgroundPrefab, new Vector2(backgroundTriggerX + backgroundOffset, 0.5f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(lastCreatedBackground.transform.position.x <= backgroundTriggerX)
        {
            lastCreatedBackground = Instantiate(backgroundPrefab, new Vector2(lastCreatedBackground.transform.position.x + backgroundOffset, 0.5f), Quaternion.identity);
        }
    }

    public void AddScore(int _score)
    {
        score += _score;
        scoreText.text = "Score:   " + score.ToString("###,###,###");
    }
}

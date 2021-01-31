using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    public TMP_Text timeCounter;

    TimeSpan timePlaying;
    bool timerGoing;

    float elapsedTime;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        timeCounter.text = "00:00:00";
        timerGoing = false;
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }

    IEnumerator UpdateTimer()
    {
        while(timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = timePlaying.ToString("hh':'mm':'ss");
            timeCounter.text = timePlayingStr;

            yield return null;
        }
    }

    void Update()
    {
        
    }
}

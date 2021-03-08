using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
 *    
 *    Provides a timer for the duration the local player has been in the game, and tracks this time on the pause menu.
 */
public class TimerController : MonoBehaviour
{
    public static TimerController instance;

    public TMP_Text timeCounter;

    TimeSpan timePlaying;
    bool timerGoing;
    public bool isMainGame = true;

    public float elapsedTime;

    /// <summary>
    /// Sets the instance of the object.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Sets the timer initial value and disables the timer on object start.
    /// </summary>
    void Start()
    {
        timeCounter.text = "00:00:00";
        timerGoing = false;
    }

    /// <summary>
    /// Starts the timer and the timer coroutine.
    /// </summary>
    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    /// <summary>
    /// Disables the timer.
    /// </summary>
    public void EndTimer()
    {
        timerGoing = false;
    }

    /// <summary>
    /// Adds the time since last check to the timer value.
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateTimer()
    {
        while(timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = timePlaying.ToString("hh':'mm':'ss");
            if(isMainGame)
            {
                timeCounter.text = timePlayingStr;
            } else
            {
                timeCounter.text = "Time:   " + timePlayingStr;
            }
            
            
            yield return null;
        }
    }
}

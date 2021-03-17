using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 2.1.1
 *    
 *    Displays post-game information and allows the players to continue or quit.
 */
public class EndScreenManager : MonoBehaviourPunCallbacks
{
    public TMP_Text stageText;
    public TMP_Text deathText;
    public TMP_Text bestTimeText;
    public GameObject NewBestTimeText;
    TimeSpan timePlaying;
    public Button nextButton;

    private void Awake()
    {
        NewBestTimeText.SetActive(false);
        bestTimeText.text = "";
        deathText.text = GlobalVariableManager.PlayerDeaths.ToString();

        InitialiseEndScreen();
    }

    private void Update()
    {
        if(PhotonNetwork.PlayerList.Length != 2)
        {
            nextButton.interactable = false;
        }
    }

    void InitialiseEndScreen()
    {
        if (GlobalVariableManager.PreviousStage == 0)
        {
            stageText.text = "Stage 1 Completed";
            timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("level1Stage1BestTime"));
            string timePlayingStr = timePlaying.ToString("hh':'mm':'ss");
            bestTimeText.text = timePlayingStr;

            if (GlobalVariableManager.Level1TimeHasChanged)
            {
                DisplayBestTime();
            }
        }
        else if (GlobalVariableManager.PreviousStage == 1)
        {
            stageText.text = "Stage 2 Completed";
            timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("level1Stage2BestTime"));
            string timePlayingStr = timePlaying.ToString("hh':'mm':'ss");
            bestTimeText.text = timePlayingStr;

            if (GlobalVariableManager.Level1TimeHasChanged)
            {
                DisplayBestTime();
            }
        }
        else if (GlobalVariableManager.PreviousStage == 2)
        {
            stageText.text = "Stage 3 Completed";
            timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("level1Stage3BestTime"));
            string timePlayingStr = timePlaying.ToString("hh':'mm':'ss");
            bestTimeText.text = timePlayingStr;

            if (GlobalVariableManager.Level1TimeHasChanged)
            {
                DisplayBestTime();
            }
        }
        else if (GlobalVariableManager.PreviousStage == 3)
        {
            stageText.text = "Stage 4 Completed";
            timePlaying = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("level1Stage4BestTime"));
            string timePlayingStr = timePlaying.ToString("hh':'mm':'ss");
            bestTimeText.text = timePlayingStr;

            if (GlobalVariableManager.Level1TimeHasChanged)
            {
                DisplayBestTime();
            }
        }
    }

    void DisplayBestTime()
    {
        NewBestTimeText.SetActive(true);
    }

    /// <summary>
    /// When the player leaves the room, load the main menu.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Triggers the player to leave the room.
    /// </summary>
    public void exitGame()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void NextLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(1);
        }
    }
}

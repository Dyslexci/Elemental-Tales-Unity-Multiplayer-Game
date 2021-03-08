﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.2
 *    
 *    Ends the game when the player enters the endgame region.
 */
public class endGame : MonoBehaviourPunCallbacks
{
    int noPlayersInEndCollider = 0;
    public CanvasGroup fadeToBlackPanel;
    bool hasEnded;

    public GameObject playerLeftObject;
    public TMP_Text playerLeftText;
    public CanvasGroup panel;
    public TimerController timerController;

    public TMP_Text endgameTextProphecy1;
    public TMP_Text endgameTextProphecy2;
    public TMP_Text endgameText3;
    public TMP_Text endgameText4;
    public TMP_Text endgameTextCallFeel;
    public TMP_Text endgameTextNextLoc;
    public CanvasGroup endgamePanel1;
    public CanvasGroup endgamePanel2;
    public CanvasGroup endgamePanel3;
    public CanvasGroup endgamePanel4;
    public CanvasGroup endgamePanel5;
    public CanvasGroup endgamePanel6;
    public CanvasGroup endgamePanelContainer;

    /// <summary>
    /// Loads the main menu when the player has successfully left the room.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        playerLeftObject =  GameObject.Find("Game Manager").GetComponent<GameMaster>().playerLeftObject;
        playerLeftText = GameObject.Find("Game Manager").GetComponent<GameMaster>().playerLeftText;
        panel = GameObject.Find("Game Manager").GetComponent<GameMaster>().panel;
    }

    private void FixedUpdate()
    {
        if(noPlayersInEndCollider == 2 && !hasEnded)
        {
            hasEnded = true;
            Debug.Log("Two players in the end zone");
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Master client is now calling the RPC");
                photonView.RPC("TriggerEndGame", RpcTarget.AllBuffered, GlobalVariableManager.Level1Stage);
            }
        }
    }

    [PunRPC] private void TriggerEndGame(int currentStage)
    {
        Debug.Log("PUN: TriggerEndGame() has been called.");
        GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<PlayerInput>().hasControl = false;
        StartCoroutine(FadeToBlackQuit());
        GlobalVariableManager.Level1Stage = currentStage + 1;
        PlayerPrefs.SetInt("level1Stage", currentStage + 1);
        timerController.EndTimer();

        if(currentStage == 0)
        {
            float previousBest = GlobalVariableManager.Level1Stage1BestTime;
            if (timerController.elapsedTime > previousBest)
            {
                GlobalVariableManager.Level1Stage1BestTime = timerController.elapsedTime;
                PlayerPrefs.SetFloat("level1Stage1BestTime", timerController.elapsedTime);
                GlobalVariableManager.Level1TimeHasChanged = true;
            }
        } else if(currentStage == 1)
        {
            float previousBest = GlobalVariableManager.Level1Stage2BestTime;
            if (timerController.elapsedTime > previousBest)
            {
                GlobalVariableManager.Level1Stage2BestTime = timerController.elapsedTime;
                PlayerPrefs.SetFloat("level1Stage2BestTime", timerController.elapsedTime);
                GlobalVariableManager.Level1TimeHasChanged = true;
            }
        } else if(currentStage == 2)
        {
            float previousBest = GlobalVariableManager.Level1Stage3BestTime;
            if (timerController.elapsedTime > previousBest)
            {
                GlobalVariableManager.Level1Stage3BestTime = timerController.elapsedTime;
                PlayerPrefs.SetFloat("level1Stage3BestTime", timerController.elapsedTime);
                GlobalVariableManager.Level1TimeHasChanged = true;
            }
        } else if(currentStage == 3)
        {
            float previousBest = GlobalVariableManager.Level1BestTime;
            if (timerController.elapsedTime > previousBest)
            {
                GlobalVariableManager.Level1BestTime = timerController.elapsedTime;
                PlayerPrefs.SetFloat("level1BestTime", timerController.elapsedTime);
                GlobalVariableManager.Level1TimeHasChanged = true;
                GlobalVariableManager.Level1Stage = 3;
                PlayerPrefs.SetInt("level1Stage", 3);
            }
        }
    }

    /// <summary>
    /// When the player has entered the collider region, disconnect them from the room and load the EndgameScene scene.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(PhotonNetwork.PlayerList.Length == 1)
            {
                noPlayersInEndCollider = 2;
                return;
            }


            noPlayersInEndCollider += 1;
            if(noPlayersInEndCollider == 1)
            {
                playerLeftObject.SetActive(true);
                if (collision.gameObject.GetPhotonView().IsMine)
                {
                    playerLeftText.text = "Waiting for <color=#ffeb04>" + PhotonNetwork.PlayerListOthers[0].NickName + " <color=#ffffff>to finish the level...";
                } else
                {
                    playerLeftText.text = "<color=#ffeb04>" + PhotonNetwork.PlayerListOthers[0].NickName + " <color=#ffffff>is waiting for you at the end of the level!";
                }
            } else
            {
                playerLeftObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            noPlayersInEndCollider -= 1;
        }
    }

    /// <summary>
    /// Fades the HUD to black and then exits the game.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeToBlackQuit()
    {
        fadeToBlackPanel.alpha = 0;
        fadeToBlackPanel.gameObject.SetActive(true);
        while (fadeToBlackPanel.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            fadeToBlackPanel.alpha += 0.05f;
        }
        SetTextContents(GlobalVariableManager.Level1Stage);
        while(endgamePanel1.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel1.alpha += 0.05f;
        }
        yield return new WaitForSeconds(.5f);
        while (endgamePanel2.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel2.alpha += 0.05f;
        }
        yield return new WaitForSeconds(1);
        while (endgamePanel3.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel3.alpha += 0.05f;
        }
        yield return new WaitForSeconds(.5f);
        while (endgamePanel4.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel4.alpha += 0.05f;
        }
        yield return new WaitForSeconds(1);
        while (endgamePanel5.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel5.alpha += 0.05f;
        }
        yield return new WaitForSeconds(1);
        while (endgamePanel6.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel6.alpha += 0.05f;
        }
        yield return new WaitForSeconds(3);

        EndSession();
    }

    void SetTextContents(int currentStage)
    {
        if(currentStage == 0)
        {
            endgameTextProphecy1.text = "All of this has happened before";
            endgameTextProphecy2.text = "And all of this will happen again";
            endgameText3.text = "Was this enough?";
            endgameText4.text = "Will the next time be enough?";
            endgameTextCallFeel.text = "You feel a call...";
            endgameTextNextLoc.text = "MOONLIGHT GROTTO";
        } else if(currentStage == 1)
        {
            endgameTextProphecy1.text = "All of this has happened before";
            endgameTextProphecy2.text = "And all of this will happen again";
            endgameText3.text = "Was this enough?";
            endgameText4.text = "Will the next time be enough?";
            endgameTextCallFeel.text = "You feel a call...";
            endgameTextNextLoc.text = "MURKY CAVES";
        } else if(currentStage == 2)
        {
            endgameTextProphecy1.text = "All of this has happened before";
            endgameTextProphecy2.text = "And all of this will happen again";
            endgameText3.text = "Was this enough?";
            endgameText4.text = "Will the next time be enough?";
            endgameTextCallFeel.text = "You feel a call...";
            endgameTextNextLoc.text = "ALLTREE HOLLOW";
        } else if(currentStage == 3)
        {
            endgameTextProphecy1.text = "All of this has happened before";
            endgameTextProphecy2.text = "But all of this need not happen again";
            endgameText3.text = "You could go again...";
            endgameText4.text = "You could stop now...";
            endgameTextCallFeel.text = "";
            endgameTextNextLoc.text = "AGAIN";
        }
        endgamePanel1.alpha = 0;
        endgamePanel2.alpha = 0;
        endgamePanel3.alpha = 0;
        endgamePanel4.alpha = 0;
        endgamePanel5.alpha = 0;
        endgamePanel6.alpha = 0;
    }

    void EndSession()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel("EndgameScene");
        }
    }
}

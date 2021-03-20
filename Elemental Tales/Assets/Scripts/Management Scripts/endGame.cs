using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.1.4
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

    /// <summary>
    /// Triggers the end of the game for both players, loading the endgame stage and setting both players to the new current level, as dictated by the current level.
    /// </summary>
    /// <param name="currentStage"></param>
    [PunRPC] private void TriggerEndGame(int currentStage)
    {
        Debug.Log("PUN: TriggerEndGame() has been called.");
        GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<PlayerInput>().hasControl = false;
        StartCoroutine(FadeToBlackQuit());
        GlobalVariableManager.Level1Stage = currentStage + 1;
        GlobalVariableManager.PreviousStage = currentStage;
        GlobalVariableManager.PlayerDeaths = GameObject.Find("Game Manager").GetComponent<GameMaster>().localPlayerDeaths;
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
        SetTextContents(GlobalVariableManager.Level1Stage);
        fadeToBlackPanel.alpha = 0;
        fadeToBlackPanel.gameObject.SetActive(true);
        while (fadeToBlackPanel.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            fadeToBlackPanel.alpha += 0.05f;
        }
        yield return new WaitForSeconds(.5f);
        while(endgamePanel1.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel1.alpha += 0.015f;
        }
        yield return new WaitForSeconds(.5f);
        while (endgamePanel2.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel2.alpha += 0.015f;
        }
        yield return new WaitForSeconds(1);
        while (endgamePanel3.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel3.alpha += 0.015f;
        }
        yield return new WaitForSeconds(.5f);
        while (endgamePanel4.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel4.alpha += 0.015f;
        }
        yield return new WaitForSeconds(1);
        while (endgamePanel5.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel5.alpha += 0.015f;
        }
        yield return new WaitForSeconds(1);
        while (endgamePanel6.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel6.alpha += 0.0075f;
        }
        yield return new WaitForSeconds(3);

        while(endgamePanel1.alpha > 0)
        {
            yield return new WaitForEndOfFrame();
            endgamePanel1.alpha -= .015f;
            endgamePanel2.alpha -= .015f;
            endgamePanel3.alpha -= .015f;
            endgamePanel4.alpha -= .015f;
            endgamePanel5.alpha -= .015f;
            endgamePanel6.alpha -= .015f;
        }

        EndSession();
    }

    /// <summary>
    /// Refactored by Adnan
    /// Sets the content of the text fields for the end-game cinematic.
    /// </summary>
    /// <param name="currentStage"></param>
    void SetTextContents(int currentStage)
    {
        string p1 = "All of this has happened before";
        string p2 = "And all of this will happen again";
        string cFeel = "Was this enough?";
        string egt4 = "Will the next time be enough?";
        string egcf = "You feel a call...";
        string location = "";

        location = 
            currentStage == 0 ? "THORNY DEPTHS" :
            currentStage == 1 ? "MURKY CAVES" :
            currentStage == 2 ? "ALLTREE HOLLOW" :
            "AGAIN";

        endgameTextProphecy1.text = p1;
        endgameTextProphecy2.text = currentStage == 3 ? "But all of this need not happen again" : p2;
        endgameText3.text = currentStage == 3 ? "You could go again..." : cFeel;
        endgameText4.text = currentStage == 3 ? "You could stop now..." : egt4;
        endgameTextCallFeel.text = currentStage == 3 ? "" : egcf;
        endgameTextNextLoc.text = location;

        endgamePanel1.alpha = 0;
        endgamePanel2.alpha = 0;
        endgamePanel3.alpha = 0;
        endgamePanel4.alpha = 0;
        endgamePanel5.alpha = 0;
        endgamePanel6.alpha = 0;
    }

    /// <summary>
    /// Ends the session for both players.
    /// </summary>
    void EndSession()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel("EndgameScene");
        }
    }
}

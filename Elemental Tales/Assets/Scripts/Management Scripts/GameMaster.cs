﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Stores global variables, player checkpoints and location for loading and saving, player scores, and etc. Created for all static variables and functions.
 */

public class GameMaster : MonoBehaviourPunCallbacks
{
    // called when the player leaves their current room
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    // called with a player that is not the local player leaves the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        StartCoroutine(WaitHidePlayerLeft(otherPlayer));
    }

    private int collectible1;
    private GameObject playerObject;

    private static bool pausedGame = false;
    [SerializeField] private GameObject pauseMenuUI;
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;
    [SerializeField] private GameObject optionsMenuUI;

    public int localPlayerDeaths;
    public int otherPlayerDeaths;

    public GameObject playerLeftObject;
    public TMP_Text playerLeftText;
    public CanvasGroup panel;

    private Transform lastCheckpoint;

    public AudioSource openDoorSound;
    public AudioSource collectGem1Sound;
    public AudioSource hintSound;

    float playedTime;

    TimerController timer;

    // initialises various game states
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        playerLeftObject.SetActive(false);
        timer = GetComponent<TimerController>();
        timer.BeginTimer();

        collectible1 = 0;

        if(playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        } else
        {
            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(spawnPoint1.position.x, spawnPoint1.position.y, 0f), Quaternion.identity, 0);
                lastCheckpoint = spawnPoint1;
            } else
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(spawnPoint2.position.x, spawnPoint2.position.y, 0f), Quaternion.identity, 0);
                lastCheckpoint = spawnPoint2;
            }
            Debug.LogFormat("We are Instantiating LocalPlayer");
        }
    }

    public void PauseGame()
    {
        if (pausedGame)
        {
            resumeGame();
        }
        else
        {
            Pause();
        }
    }

    public void setPlayer(GameObject player)
    {
        playerObject = player;
        Debug.Log("Player has been successfully assigned to the game master - " + playerObject.name);
    }

    public GameObject getPlayer()
    {
        return playerObject;
    }

    public void setCheckpoint(Transform newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
    }

    public void respawn()
    {
        Debug.Log("GameMaster: respawn() has been called.");
        localPlayerDeaths++;
        //photonView.RPC("AddOppositePlayerDeath", RpcTarget.OthersBuffered);
        StartCoroutine(Respawn());
    }

    public void addCollectible1()
    {
        collectible1++;
        print(collectible1);
    }

    public int getCollectible1()
    {
        return collectible1;
    }

    public void resumeGame()
    {
        pausedGame = false;
        pauseMenuUI.SetActive(false);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        pausedGame = true;
    }

    public void optionsMenu()
    {
        Debug.Log("Options pressed");
        //optionsMenuUI.SetActive(true);
        //pauseMenuUI.SetActive(false);
    }

    // If statement will attempt to leave correctly if the player is joined in a room and ready to continue as normal, otherwise it will assume the network has crashed out and return to main
    public void leaveGame()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("Quitting Game....");
            PhotonNetwork.LeaveRoom();
        } else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void restartLevel()
    {
        Debug.Log("Restart pressed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [PunRPC]
    private void AddOppositePlayerDeath()
    {
        Debug.Log("PUN: AddOppositePlayerDeath() has been called.");
        otherPlayerDeaths++;
    }

    IEnumerator WaitHidePlayerLeft(Player otherPlayer)
    {
        playerLeftObject.SetActive(true);
        playerLeftText.text = "<color=#ffeb04>" + otherPlayer.NickName + " <color=#ffffff>has left the game";
        yield return new WaitForSeconds(4);
        StartCoroutine(FadePanel());
    }

    IEnumerator FadePanel()
    {
        while (panel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            panel.alpha -= 0.05f;
        }

        playerLeftObject.SetActive(false);
        panel.alpha = 1;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2.0f);
        playerObject.transform.SetPositionAndRotation(new Vector3(lastCheckpoint.position.x, lastCheckpoint.position.y, 0f), Quaternion.identity);
        playerObject.GetComponent<StatController>().resetPlayerAfterDeath();
    }
}

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
                photonView.RPC("TriggerEndGame", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC] private void TriggerEndGame()
    {
        Debug.Log("PUN: TriggerEndGame() has been called.");
        GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<PlayerInput>().hasControl = false;
        StartCoroutine(FadeToBlackQuit());
    }

    /// <summary>
    /// When the player has entered the collider region, disconnect them from the room and load the EndgameScene scene.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
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
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel("EndgameScene");
        }
    }
}

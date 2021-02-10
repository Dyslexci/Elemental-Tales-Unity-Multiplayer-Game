using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 2.0.0
 *    
 *    Stores global variables, player checkpoints and location for loading and saving, player scores, and etc. Created for all static variables and functions.
 */

public class GameMaster : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Loads the main menu when the player has left the network room.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Displays a message to the player when the other player has left the network room.
    /// </summary>
    /// <param name="otherPlayer"></param>
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
    public CanvasGroup fadeToBlackPanel;

    private Transform lastCheckpoint;

    public AudioSource openDoorSound;
    public AudioSource collectGem1Sound;
    public AudioSource hintSound;

    float playedTime;

    TimerController timer;

    float tempCameraStartPos = 101.8f;
    public Camera tempCamera;
    public Camera mainCam;
    public CanvasGroup tempCamPanel;
    public CanvasGroup HUDPanel;
    public CanvasGroup tempCamTextPanel;

    public AudioSource music;
    public AudioSource ambientSound;
    float musicStartVolume;
    float ambientSoundStartVolume;

    /// <summary>
    /// Initialises various game states and instantiates the player prefabs, allocating one to the local player and one to the other player.
    /// </summary>
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        playerLeftObject.SetActive(false);
        timer = GetComponent<TimerController>();
        timer.BeginTimer();

        musicStartVolume = music.volume;
        ambientSoundStartVolume = ambientSound.volume;

        fadeToBlackPanel.alpha = 0;
        fadeToBlackPanel.gameObject.SetActive(false);

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
        StartCoroutine(StartSceneCinematic());
    }

    private void Update()
    {
        if(!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            Debug.Log("The game manager was inactive, resetting...");
        }
    }

    /// <summary>
    /// Toggles the pause game state.
    /// </summary>
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

    /// <summary>
    /// Called by the CharacterControllerRaycast to store the local player prefab in the game manager to allow integration between the player object and unrelated scripts.
    /// </summary>
    /// <param name="player"></param>
    public void setPlayer(GameObject player)
    {
        playerObject = player;
        Debug.Log("Player has been successfully assigned to the game master - " + playerObject.name);
    }

    /// <summary>
    /// Returns the local player object.
    /// </summary>
    /// <returns></returns>
    public GameObject getPlayer()
    {
        return playerObject;
    }

    /// <summary>
    /// Sets the checkpoint to the parameter.
    /// </summary>
    /// <param name="newCheckpoint"></param>
    public void setCheckpoint(Transform newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
    }

    /// <summary>
    /// Respawns the local player when called.
    /// </summary>
    public void respawn()
    {
        Debug.Log("GameMaster: respawn() has been called.");
        localPlayerDeaths++;
        //photonView.RPC("AddOppositePlayerDeath", RpcTarget.OthersBuffered);
        StartCoroutine(Respawn());
    }

    /// <summary>
    /// Adds 1 to the collectible1 counter.
    /// </summary>
    public void addCollectible1()
    {
        collectible1++;
    }

    /// <summary>
    /// Returns the total collectible1 count.
    /// </summary>
    /// <returns></returns>
    public int getCollectible1()
    {
        return collectible1;
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void resumeGame()
    {
        pausedGame = false;
        pauseMenuUI.SetActive(false);
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        pausedGame = true;
    }

    /// <summary>
    /// Opens the options menu.
    /// </summary>
    public void optionsMenu()
    {
        Debug.Log("Options pressed");
        //optionsMenuUI.SetActive(true);
        //pauseMenuUI.SetActive(false);
    }

    /// <summary>
    /// Will attempt to leave the room correctly is the player is in a room and ready to leave as normal, otherwise assuming the network has broken and will return to main scene.
    /// </summary>
    public void leaveGame()
    {
        playerObject.GetComponent<PlayerInput>().StartCoroutine(FadeToBlackQuit());
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// RPC to add 1 to the opposite player deaths.
    /// </summary>
    [PunRPC]
    private void AddOppositePlayerDeath()
    {
        Debug.Log("PUN: AddOppositePlayerDeath() has been called.");
        otherPlayerDeaths++;
    }

    /// <summary>
    /// Starts displaying the tooltip announcing the other player has left the room.
    /// </summary>
    /// <param name="otherPlayer"></param>
    /// <returns></returns>
    IEnumerator WaitHidePlayerLeft(Player otherPlayer)
    {
        playerLeftObject.SetActive(true);
        playerLeftText.text = "<color=#ffeb04>" + otherPlayer.NickName + " <color=#ffffff>has left the game";
        yield return new WaitForSeconds(4);
        StartCoroutine(FadePanel());
    }

    /// <summary>
    /// Fades out the leaving tooltip.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Respawns the player after a certain amount of time.
    /// </summary>
    /// <returns></returns>
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2.0f);
        playerObject.transform.SetPositionAndRotation(new Vector3(lastCheckpoint.position.x, lastCheckpoint.position.y, 0f), Quaternion.identity);
        playerObject.GetComponent<StatController>().resetPlayerAfterDeath();
    }

    /// <summary>
    /// Fades the HUD to black and then exits the game.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeToBlackQuit()
    {
        fadeToBlackPanel.alpha = 0;
        fadeToBlackPanel.gameObject.SetActive(true);
        while(fadeToBlackPanel.alpha < 1)
        {
            yield return new WaitForEndOfFrame();
            fadeToBlackPanel.alpha += 0.05f;
        }
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("Quitting Game....");
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    /// <summary>
    /// Pans the temporary camera down from the starting position to the players position and fades in the HUD.
    /// </summary>
    /// <returns></returns>
    IEnumerator StartSceneCinematic()
    {
        mainCam.gameObject.SetActive(false);
        tempCamera.gameObject.SetActive(true);
        tempCamTextPanel.gameObject.SetActive(true);
        tempCamTextPanel.alpha = 0;
        HUDPanel.alpha = 0;
        music.volume = 0;
        ambientSound.volume = 0;
        float increaseAmount = .1f;
        while(playerObject == null)
        {
            yield return new WaitForFixedUpdate();
        }
        playerObject.GetComponent<PlayerInput>().hasControl = false;
        while(tempCamTextPanel.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            tempCamTextPanel.alpha += 0.03f;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeInFromBlack());
        tempCamera.transform.position = new Vector3(playerObject.transform.position.x, tempCameraStartPos, -10);
        while(tempCamera.transform.position.y > playerObject.transform.position.y)
        {
            yield return new WaitForFixedUpdate();
            tempCamera.transform.position = new Vector3(tempCamera.transform.position.x, tempCamera.transform.position.y - increaseAmount, tempCamera.transform.position.z);
            if (tempCamera.transform.position.y > 83.5f)
            {
                increaseAmount += 0.005f;
            }
            if(tempCamera.transform.position.y < playerObject.transform.position.y + 19f && increaseAmount > 0.01f)
            {
                increaseAmount -= 0.005f;
                if(tempCamTextPanel.alpha == 1)
                {
                    StartCoroutine(FadeOutTempTextPanel());
                }
            }
        }
        tempCamera.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
        playerObject.GetComponent<PlayerInput>().hasControl = true;
        while (HUDPanel.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            HUDPanel.alpha += 0.03f;
        }
        HUDPanel.alpha = 1;
    }

    /// <summary>
    /// Fades the camera in from a black screen, and the music/global sound effects from silence to the default volume.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeInFromBlack()
    {
        tempCamPanel.alpha = 1;
        tempCamPanel.gameObject.SetActive(true);
        while(tempCamPanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            tempCamPanel.alpha -= 0.008f;
            if(music.volume < musicStartVolume)
            {
                music.volume += 0.008f * musicStartVolume;
            }
            if(ambientSound.volume < ambientSoundStartVolume)
            {
                ambientSound.volume += 0.008f * ambientSoundStartVolume;
            }
        }
        music.volume = musicStartVolume;
        ambientSound.volume = ambientSoundStartVolume;
        tempCamPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fades out the text at the top of the screen during the intro cinematic, displaying the name of the level.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutTempTextPanel()
    {
        while(tempCamTextPanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            tempCamTextPanel.alpha -= 0.03f;
        }
        tempCamTextPanel.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 2.2.1
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

    [Header("Generic Setup")]
    private static bool pausedGame = false;
    [SerializeField] private GameObject pauseMenuHolder;
    public GameObject HUDCanvas;
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public GameObject arrow;
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;
    [SerializeField] private GameObject optionsMenuUI;
    public CanvasGroup pauseMenuPanel;

    [Header("Death Variables")]
    public int localPlayerDeaths;
    public TMP_Text playerDeathCounter;
    public int otherPlayerDeaths;

    [Header("Collectible Variables")]
    public TMP_Text collectible1Counter;
    public int totalCollectible1;

    [Header("Canvas Variables")]
    public GameObject playerLeftObject;
    public TMP_Text playerLeftText;
    public CanvasGroup panel;
    public CanvasGroup fadeToBlackPanel;
    public CanvasGroup HUDBlackPanel;
    public CanvasGroup HUDWhitePanel;
    public Light2D deathLight;

    public CanvasGroup deathMessage1Panel;
    public CanvasGroup deathMessage2Panel;
    public CanvasGroup deathMessage3Panel;
    public CanvasGroup deathMessage4Panel;

    public CanvasGroup pauseQuitPanel;

    [Header("Checkpoint Variables")]
    private Transform lastCheckpoint;
    private ArrayList checkpointsVisited;
    private int numCheckpointsVisited;
    public int totalCheckpoints;
    public TMP_Text percentageTracker;

    [Header("Audio Variables")]
    public AudioSource openDoorSound;
    public AudioSource collectGem1Sound;
    public AudioSource hintSound;
    public AudioSource music;
    public AudioSource ambientSound;
    float musicStartVolume;
    float ambientSoundStartVolume;

    float playedTime;

    TimerController timer;

    [Header("Camera Variables")]
    float tempCameraStartPos = 101.8f;
    public Camera tempCamera;
    public Camera mainCam;
    public CanvasGroup tempCamPanel;
    public CanvasGroup HUDPanel;
    public CanvasGroup tempCamTextPanel;

    public bool playerHasInstantiated = false;

    /// <summary>
    /// Initialises various game states and instantiates the player prefabs, allocating one to the local player and one to the other player.
    /// </summary>
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        checkpointsVisited = new ArrayList();

        HUDCanvas.SetActive(true);
        pauseMenuHolder.SetActive(false);
        pauseQuitPanel.gameObject.SetActive(false);
        optionsMenuUI.SetActive(false);
        playerLeftObject.SetActive(false);
        timer = GetComponent<TimerController>();
        timer.BeginTimer();
        arrow.SetActive(false);
        playerDeathCounter.text = "0";

        musicStartVolume = music.volume;
        ambientSoundStartVolume = ambientSound.volume;

        HUDBlackPanel.alpha = 0;
        HUDBlackPanel.gameObject.SetActive(false);
        HUDWhitePanel.gameObject.SetActive(false);
        fadeToBlackPanel.alpha = 0;
        fadeToBlackPanel.gameObject.SetActive(false);
        deathMessage1Panel.gameObject.SetActive(false);
        deathMessage2Panel.gameObject.SetActive(false);
        deathMessage3Panel.gameObject.SetActive(false);
        deathMessage4Panel.gameObject.SetActive(false);

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
        playerHasInstantiated = true;
        Debug.Log("GAMEMASTER: Player has been successfully instantiated and assigned to the game master");
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
        if(checkpointsVisited.Contains(newCheckpoint))
        {
            return;
        } else
        {
            checkpointsVisited.Add(newCheckpoint);
            numCheckpointsVisited++;
            percentageTracker.text = Mathf.RoundToInt((numCheckpointsVisited / totalCheckpoints) * 100) + "%";
        }
    }

    /// <summary>
    /// Respawns the local player when called.
    /// </summary>
    public void respawn()
    {
        Debug.Log("GameMaster: respawn() has been called.");
        localPlayerDeaths++;
        playerDeathCounter.text = localPlayerDeaths.ToString();
        //photonView.RPC("AddOppositePlayerDeath", RpcTarget.OthersBuffered);
        StartCoroutine(RespawnAnimation());
    }

    /// <summary>
    /// Fades in parts of the death screen one after the other to display text, the death counter, etc
    /// </summary>
    /// <returns></returns>
    IEnumerator RespawnAnimation()
    {
        HUDBlackPanel.alpha = 0;
        HUDBlackPanel.gameObject.SetActive(true);
        deathLight.intensity = 0;
        deathLight.gameObject.SetActive(false);
        playerObject.SetActive(false);
        while (HUDBlackPanel.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            HUDPanel.alpha -= .05f;
            HUDBlackPanel.alpha += .05f;
            music.volume -= musicStartVolume / 20;
            ambientSound.volume -= ambientSoundStartVolume / 20;
        }
        ambientSound.volume = 0;
        music.volume = 0;
        yield return new WaitForSeconds(.5f);

        deathMessage1Panel.gameObject.SetActive(true);
        deathMessage1Panel.alpha = 0;
        deathMessage2Panel.gameObject.SetActive(true);
        deathMessage2Panel.alpha = 0;
        deathMessage3Panel.gameObject.SetActive(true);
        deathMessage3Panel.alpha = 0;
        deathMessage4Panel.gameObject.SetActive(true);
        deathMessage4Panel.alpha = 0;
        if (localPlayerDeaths == 1)
        {
            deathMessage2Panel.GetComponentInChildren<TMP_Text>().text = "So this is how it ends...";
            deathMessage3Panel.GetComponentInChildren<TMP_Text>().text = "You only wish your partner could be here, with you";
            deathMessage4Panel.GetComponentInChildren<TMP_Text>().text = "What?";
            while(deathMessage2Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage2Panel.alpha += .01f;
            }
            yield return new WaitForSeconds(.5f);
            while (deathMessage3Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage3Panel.alpha += .01f;
            }
            yield return new WaitForSeconds(.5f);
            while (deathMessage4Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage4Panel.alpha += .01f;
            }
            yield return new WaitForSeconds(.5f);
        } else
        {
            deathMessage1Panel.GetComponentInChildren<TMP_Text>().text = "And here is the darkness again...";
            deathMessage2Panel.GetComponentInChildren<TMP_Text>().text = "How many times, now?";
            deathMessage3Panel.GetComponentInChildren<TMP_Text>().text = localPlayerDeaths + "?";
            deathMessage4Panel.GetComponentInChildren<TMP_Text>().text = "And here we go again...";
            while (deathMessage1Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage1Panel.alpha += .01f;
            }
            yield return new WaitForSeconds(.5f);
            while (deathMessage2Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage2Panel.alpha += .01f;
            }
            yield return new WaitForSeconds(.5f);
            while (deathMessage3Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage3Panel.alpha += .01f;
            }
            yield return new WaitForSeconds(.5f);
            while (deathMessage4Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage4Panel.alpha += .01f;
            }
            yield return new WaitForSeconds(.5f);
        }
        StartCoroutine(FinishRespawnAnimation());
    }

    /// <summary>
    /// Begins fading out various parts of the death screen to smoothly return to the game
    /// </summary>
    /// <returns></returns>
    IEnumerator FinishRespawnAnimation()
    {
        deathLight.gameObject.SetActive(true);
        playerObject.transform.SetPositionAndRotation(new Vector3(lastCheckpoint.position.x, lastCheckpoint.position.y, 0f), Quaternion.identity);
        playerObject.GetComponent<StatController>().resetPlayerAfterDeath();
        while (deathLight.intensity < 1)
        {
            yield return new WaitForFixedUpdate();
            deathLight.intensity += .025f;
        }
        while (deathLight.intensity < 9)
        {
            yield return new WaitForFixedUpdate();
            deathLight.intensity += 0.25f;
        }
        HUDWhitePanel.alpha = 1;
        HUDWhitePanel.gameObject.SetActive(true);
        HUDBlackPanel.gameObject.SetActive(false);
        HUDBlackPanel.alpha = 0;
        deathMessage1Panel.gameObject.SetActive(false);
        deathMessage2Panel.gameObject.SetActive(false);
        deathMessage3Panel.gameObject.SetActive(false);
        deathMessage4Panel.gameObject.SetActive(false);
        deathLight.gameObject.SetActive(false);
        HUDPanel.alpha = 1;
        playerObject.SetActive(true);
        while (HUDWhitePanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            HUDWhitePanel.alpha -= .025f;
            music.volume += musicStartVolume / 40;
            ambientSound.volume += ambientSoundStartVolume / 40;
        }
        ambientSound.volume = ambientSoundStartVolume;
        music.volume = musicStartVolume;
        HUDWhitePanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Adds 1 to the collectible1 counter.
    /// </summary>
    public void addCollectible1()
    {
        collectible1++;
        collectible1Counter.text = collectible1 + "/" + totalCollectible1;
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
        playerObject.GetComponent<PlayerInput>().hasControl = true;
        pausedGame = false;
        pauseMenuHolder.SetActive(false);
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    void Pause()
    {
        playerObject.GetComponent<PlayerInput>().hasControl = false;
        GetComponent<PauseMenuManager>().skillTitle.text = "";
        pauseMenuHolder.SetActive(true);
        pausedGame = true;
    }

    /// <summary>
    /// Opens the options menu.
    /// </summary>
    public void optionsMenu()
    {
        
        optionsMenuUI.gameObject.SetActive(true);
        pauseMenuPanel.gameObject.SetActive(false);
    }

    public void CloseOptionsMenu()
    {
        optionsMenuUI.gameObject.SetActive(false);
        pauseMenuPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// Will attempt to leave the room correctly is the player is in a room and ready to leave as normal, otherwise assuming the network has broken and will return to main scene.
    /// </summary>
    public void leaveGame()
    {
        StartCoroutine(FadePauseQuitPanelIn());
    }

    /// <summary>
    /// Fades in the quit confirmation screen
    /// </summary>
    /// <returns></returns>
    IEnumerator FadePauseQuitPanelIn()
    {
        pauseQuitPanel.alpha = 0;
        pauseQuitPanel.gameObject.SetActive(true);
        while (pauseQuitPanel.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            pauseQuitPanel.alpha += .05f;
        }
    }

    /// <summary>
    /// Quits the game back to the main menu
    /// </summary>
    public void QuitToMenu()
    {
        playerObject.GetComponent<PlayerInput>().StartCoroutine(FadeToBlackQuit());
    }

    /// <summary>
    /// Quits the application to desktop - may negatively affect second player
    /// </summary>
    public void QuitToDesktop()
    {
        Application.Quit();
    }

    /// <summary>
    /// Cancels the quit confirmation
    /// </summary>
    public void QuitCancel()
    {
        StartCoroutine(FadePauseQuitPanelOut());
    }

    /// <summary>
    /// Fades out the quit confirmation screen
    /// </summary>
    /// <returns></returns>
    IEnumerator FadePauseQuitPanelOut()
    {
        pauseQuitPanel.alpha = 1;
        while (pauseQuitPanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            pauseQuitPanel.alpha -= .05f;
        }
        pauseQuitPanel.gameObject.SetActive(false);
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

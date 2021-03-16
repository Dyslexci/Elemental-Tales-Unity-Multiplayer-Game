﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 3.3.2
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
    public int mapStage = 0;
    public HandleZoneChanges zoneChanger;
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
    public doorLeverInput[] doorArray;
    SpriteRenderer playerSprite;

    [Header("Death Variables")]
    public int localPlayerDeaths;
    public TMP_Text playerDeathCounter;
    public int otherPlayerDeaths;

    [Header("Collectible Variables")]
    public TMP_Text collectible1Counter;
    public int totalCollectible1;
    public TMP_Text healthUpgradeCounter;
    int healthShards = 0;
    public UnityEngine.UI.Image collectibleImage;

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

    public TMP_Text deathMessage1;
    public TMP_Text deathMessage2;
    public TMP_Text deathMessage3;
    public TMP_Text deathMessage4;

    public CanvasGroup pauseQuitPanel;

    public CanvasGroup leverPromptPanel;

    [Header("Checkpoint Variables")]
    private Transform lastCheckpoint;
    public float numberOfCheckpointsReached = 0;
    public float totalCheckpoints;
    public TMP_Text percentageTracker;

    [Header("Game Repetition Variables")]
    public GameObject blocker1;
    public GameObject blocker2;
    public GameObject blocker3;
    public GameObject end1;
    public GameObject end2;
    public GameObject end3;
    public GameObject bashHolder;
    public NPCBehaviourTemp NPCStage1;
    public NPCBehaviourTemp NPCStage2;
    public NPCBehaviourTemp NPCStage3;
    public NPCBehaviourTemp NPCStage4;
    public DialogueTrigger dialogueStage1;
    public DialogueTrigger dialogueStage2;
    public DialogueTrigger dialogueStage3;
    public DialogueTrigger dialogueStage4;

    [Header("Audio Variables")]
    public AudioSource openDoorSound;
    public AudioSource collectGem1Sound;
    public AudioSource hintSound;
    public AudioSource forestMusic;
    public AudioSource poolsMusic;
    public AudioSource grottoMusic;
    public AudioSource ambientSound;
    public AudioSource switchPull;
    public AudioSource signpostEnterRange;
    public AudioSource signpostExitRange;
    public AudioSource healthRefillCollect;
    public AudioSource healthCollector;
    public AudioSource manaCollector;
    float musicStartVolume;
    float ambientSoundStartVolume;

    [Header("Locational Variables")]
    public bool inForest = true;
    public bool inPools;
    public bool inGrotto;
    public TMP_Text areaText;
    public CanvasGroup areaTextPanel;
    public bool mistyGladesCompleted;
    public bool thornyDepthsCompleted;
    public bool starlightGrottoCompleted;
    public bool murkyCavesCompleted;
    public bool alltreeHollowCompleted;

    [Header("First-Time Variables")]
    bool hasDisplayedHealthPopup;
    bool hasDisplayedScorePopup;
    bool hasDisplayedHealthUpgradePopup;
    TMP_Text hintText;
    GameObject hintHolder;
    CanvasGroup hintPanel;
    UnityEngine.UI.Image hintImage;
    public bool hasWalked;
    public bool hasJumped;
    public bool hasDoubledJumped;
    public bool hasWallClimbed;

    [Header("Player Audio Variables")]
    public AudioSource respawnAudio;
    public AudioSource[] deathAudio;
    public AudioSource[] injuryAudio;
    public AudioSource[] elementChangeAudio;
    public AudioSource[] panCameraAudio;
    public AudioSource elementalGetStart;
    public AudioSource elementDialogueStart;

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

        zoneChanger = GetComponent<HandleZoneChanges>();

        HUDCanvas.SetActive(true);
        pauseMenuHolder.SetActive(false);
        pauseQuitPanel.gameObject.SetActive(false);
        optionsMenuUI.SetActive(false);
        playerLeftObject.SetActive(false);
        timer = GetComponent<TimerController>();
        timer.BeginTimer();
        arrow.SetActive(false);
        playerDeathCounter.text = "0";
        percentageTracker.text = "00%";

        musicStartVolume = forestMusic.volume;
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

        hintText = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintText();
        hintHolder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().GetHintHolder();
        hintImage = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintContainer();
        hintPanel = hintHolder.GetComponentInChildren<CanvasGroup>();

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

        if(PhotonNetwork.IsMasterClient)
            photonView.RPC("SetCurrentStage", RpcTarget.AllBuffered, GlobalVariableManager.Level1Stage);
    }

    private void Update()
    {
        if(!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            Debug.Log("The game manager was inactive, resetting...");
        }

        CheckLeverPromptState();
    }

    [PunRPC] private void SetCurrentStage(int currentStage)
    {
        if(currentStage == 0)
        {
            mapStage = currentStage;
            bashHolder.SetActive(false);
        } else if(currentStage == 1)
        {
            mapStage = currentStage;
            blocker1.SetActive(false);
            end1.SetActive(false);
        } else if(currentStage == 2)
        {
            mapStage = currentStage;
            blocker1.SetActive(false);
            blocker2.SetActive(false);
            end1.SetActive(false);
            end2.SetActive(false);
        } else if(currentStage == 3)
        {
            mapStage = currentStage;
            blocker1.SetActive(false);
            blocker2.SetActive(false);
            blocker3.SetActive(false);
            end1.SetActive(false);
            end2.SetActive(false);
            end3.SetActive(false);
        }
        Debug.Log("Loading level 1 with current stage set as " + currentStage);
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
    }

    /// <summary>
    /// Triggered by the CheckpointRegion script to re-draw the percentage explored text
    /// </summary>
    public void AddPercentageExplored()
    {
        percentageTracker.text = (numberOfCheckpointsReached / totalCheckpoints).ToString("0.00%");
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
        playerObject.GetComponent<PlayerManager>().DespawnPlayer();
        //playerObject.SetActive(false);
        while (HUDBlackPanel.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            HUDPanel.alpha -= .05f;
            HUDBlackPanel.alpha += .05f;
            forestMusic.volume -= musicStartVolume / 20;
            ambientSound.volume -= ambientSoundStartVolume / 20;
        }
        ambientSound.volume = 0;
        forestMusic.volume = 0;
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
            deathMessage2.text = "So this is how it ends...";
            deathMessage3.text = "You only wish your partner could be here, with you";
            deathMessage4.text = "What?";
            while(deathMessage2Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage2Panel.alpha += .03f;
            }
            yield return new WaitForSeconds(.25f);
            while (deathMessage3Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage3Panel.alpha += .03f;
            }
            yield return new WaitForSeconds(.25f);
            while (deathMessage4Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage4Panel.alpha += .03f;
            }
            yield return new WaitForSeconds(.25f);
        } else
        {
            deathMessage1.text = "And here is the darkness again...";
            deathMessage2.text = "How many times, now?";
            deathMessage3.text = localPlayerDeaths + "?";
            deathMessage4.text = "And here we go again...";
            while (deathMessage1Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage1Panel.alpha += .03f;
            }
            yield return new WaitForSeconds(.25f);
            while (deathMessage2Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage2Panel.alpha += .03f;
            }
            yield return new WaitForSeconds(.25f);
            while (deathMessage3Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage3Panel.alpha += .03f;
            }
            yield return new WaitForSeconds(.25f);
            while (deathMessage4Panel.alpha < 1)
            {
                yield return new WaitForFixedUpdate();
                deathMessage4Panel.alpha += .03f;
            }
            yield return new WaitForSeconds(.25f);
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
        playerObject.GetComponent<PlayerInputs>().velocity.x = 0;
        playerObject.GetComponent<PlayerInputs>().velocity.y = 0;
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
        playerObject.GetComponent<PlayerManager>().RespawnPlayer();
        //playerObject.SetActive(true);
        respawnAudio.Play(0);
        while (HUDWhitePanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            HUDWhitePanel.alpha -= .025f;
            forestMusic.volume += musicStartVolume / 40;
            ambientSound.volume += ambientSoundStartVolume / 40;
        }
        ambientSound.volume = ambientSoundStartVolume;
        forestMusic.volume = musicStartVolume;
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

    public void AddHealthUpgrade()
    {
        healthShards++;
        healthUpgradeCounter.text = healthShards + "/5";
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
        if (!PhotonNetwork.IsMasterClient)
            return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// RPC to add 1 to the opposite player deaths.
    /// </summary>
    [PunRPC] private void AddOppositePlayerDeath()
    {
        Debug.Log("PUN: AddOppositePlayerDeath() has been called.");
        otherPlayerDeaths++;
    }

    void CheckLeverPromptState()
    {
        int noDoorsPrompting = 0;
        for (int i = 0; i < doorArray.Length; i++)
        {
            if (doorArray[i].displayHint)
            {
                noDoorsPrompting++;
            }
        }

        if (noDoorsPrompting > 0)
        {
            leverPromptPanel.gameObject.SetActive(true);
        } else
        {
            leverPromptPanel.gameObject.SetActive(false);
        }
    }

    public void DisplayHealthPopup()
    {
        if(!hasDisplayedHealthPopup)
        {
            hintText.text = "<color=#ffffff>Bounce on these <color=#ffeb04>Mushrooms <color=#ffffff>to refill your health!";
            StopCoroutine(JumpInHintHolder());
            StopCoroutine(WaitHideHint());
            StopCoroutine(FadeHintHolder());
            StartCoroutine(WaitHideHint());
            hasDisplayedHealthPopup = true;
        }
    }

    public void DisplayScorePopup()
    {
        if(!hasDisplayedScorePopup)
        {
            hintText.text = "<color=#ffffff>Collect <color=#ffeb04>Spirit Shards <color=#ffffff>to increase your score!";
            StopCoroutine(JumpInHintHolder());
            StopCoroutine(WaitHideHint());
            StopCoroutine(FadeHintHolder());
            StartCoroutine(WaitHideHint());
            hasDisplayedScorePopup = true;
        }
    }

    public void DisplayHealthUpgradePopup()
    {
        if (!hasDisplayedHealthUpgradePopup)
        {
            hintText.text = "<color=#ffffff>Collect <color=#ffeb04>Health Shards <color=#ffffff>to increase your maximum health!";
            StopCoroutine(JumpInHintHolder());
            StopCoroutine(WaitHideHint());
            StopCoroutine(FadeHintHolder());
            StartCoroutine(WaitHideHint());
            hasDisplayedHealthUpgradePopup = true;
        }
    }

    /// <summary>
    /// Coroutine displaying the hint to the player.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitHideHint()
    {
        hintHolder.SetActive(true);
        StartCoroutine(JumpInHintHolder());
        
        yield return new WaitForSeconds(2);
        StartCoroutine(FadeHintHolder());
    }

    /// <summary>
    /// Coroutine making the hint jump into place.
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpInHintHolder()
    {
        hintImage.transform.localScale = new Vector3(5, 5, 5);

        while (hintImage.transform.localScale.x > 1)
        {
            yield return new WaitForFixedUpdate();
            hintImage.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        }
        hintImage.transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Coroutine making the hint fade out after it has been on screen enough time.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeHintHolder()
    {
        while (hintPanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            hintPanel.alpha -= 0.05f;
        }

        hintHolder.SetActive(false);
        hintPanel.alpha = 1;
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
        forestMusic.volume = 0;
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
        while (HUDPanel.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            HUDPanel.alpha += 0.03f;
        }
        HUDPanel.alpha = 1;

        if(mapStage == 0)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogueStage1.dialogue, NPCStage1, dialogueStage1.quest);
            NPCStage1.isTalking = true;
        } else if (mapStage == 1)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogueStage2.dialogue, NPCStage2, dialogueStage2.quest);
            NPCStage2.isTalking = true;
        } else if (mapStage == 2)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogueStage3.dialogue, NPCStage3, dialogueStage3.quest);
            NPCStage3.isTalking = true;
        } else if (mapStage == 3)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogueStage4.dialogue, NPCStage4, dialogueStage4.quest);
            NPCStage4.isTalking = true;
        }
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
            if(forestMusic.volume < musicStartVolume)
            {
                forestMusic.volume += 0.008f * musicStartVolume;
            }
            if(ambientSound.volume < ambientSoundStartVolume)
            {
                ambientSound.volume += 0.008f * ambientSoundStartVolume;
            }
        }
        forestMusic.volume = musicStartVolume;
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

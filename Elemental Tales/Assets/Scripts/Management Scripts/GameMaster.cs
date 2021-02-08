using System.Collections;
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
 *    @version 1.0.0
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

    private Transform lastCheckpoint;

    public AudioSource openDoorSound;
    public AudioSource collectGem1Sound;
    public AudioSource hintSound;

    float playedTime;

    TimerController timer;

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
        if(PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("Quitting Game....");
            PhotonNetwork.LeaveRoom();
        } else
        {
            SceneManager.LoadScene(0);
        }
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
}

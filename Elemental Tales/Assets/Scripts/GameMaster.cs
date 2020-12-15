using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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

    //public void onPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    playerObject = (GameObject)info.Sender.TagObject;
    //}

    private int collectible1;
    private GameObject playerObject;

    private static bool pausedGame = false;
    [SerializeField] private GameObject pauseMenuUI;
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;

    private Transform lastCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        collectible1 = 0;
        //Debug.Log(playerObject.name + " has been correctly stored in the local gamemaster");

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
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
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
            yield return new WaitForSeconds(2.0f);
            playerObject.transform.SetPositionAndRotation(new Vector3(lastCheckpoint.position.x, lastCheckpoint.position.y, 0f), Quaternion.identity);
            playerObject.GetComponent<Health>().resetPlayerAfterDeath();
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pausedGame = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        pausedGame = true;
    }

    public void loadingMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void quittingMenu()
    {
        Debug.Log("Quitting Game....");
        Application.Quit();
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Team12.ElementalTales
{
    public class pauseMenu : MonoBehaviourPunCallbacks
    {
        #region Photon Callbacks

        /// <summary>
        /// Called when the local player left the room. Needed to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        #endregion


        #region Private Serialisable Fields

        [SerializeField] private static bool pausedGame = false;

        [SerializeField] private GameObject pauseMenuUI;

        #endregion


        #region Private Methods

        private void Start()
        {
            pauseMenuUI.SetActive(false);
        }

        private void Update()
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

        private void Pause()
        {
            pauseMenuUI.SetActive(true);
            pausedGame = true;
        }

        #endregion


        #region Public Methods

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            pausedGame = false;
        }

        public void loadingMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }

        public void leaveRoom()
        {
            Debug.Log("Returning to main menu");
            PhotonNetwork.LeaveRoom();
        }

        public void restartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        #endregion
    }
}

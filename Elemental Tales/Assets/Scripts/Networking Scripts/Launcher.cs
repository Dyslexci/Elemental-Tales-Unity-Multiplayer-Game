using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Implements the lobby system and launcer for the PUN2 networking package.
 */

namespace Com.Team12.ElementalTales
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region MonoBehaviourPunCallbacks Callbacks


        /// <summary>
        /// Method called when the client disconnects from the online host server
        /// </summary>
        /// <param name="cause"></param>
        public override void OnDisconnected(DisconnectCause cause)
        {
            progressPanel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        /// <summary>
        /// Method called when the client fails to join a room.
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);
            invalidCodeText.SetActive(true);
        }

        /// <summary>
        /// Method called when the client successfully joins a room.
        /// </summary>
        public override void OnJoinedRoom()
        {
            Debug.Log("PUN: OnJoinedRoom() called by PUN. Now this client is in a room.");
            Debug.Log("In lobby: " + PhotonNetwork.InLobby);
            invalidCodeText.SetActive(false);
            progressPanel.SetActive(false);
            lobbyPanel.SetActive(true);
            controlPanel.SetActive(false);
            lobbyTitleText.text = PhotonNetwork.PlayerList[0].NickName + "'s Lobby";
            codeText.text = "" + PhotonNetwork.CurrentRoom.Name;
            if (PhotonNetwork.PlayerList.Length == 1)
            {
                lobbyPlayer1Text.text = PhotonNetwork.NickName;
            } else
            {
                lobbyPlayer1Text.text = PhotonNetwork.PlayerList[0].NickName;
                lobbyPlayer2Text.text = PhotonNetwork.NickName;
            }
        }

        /// <summary>
        /// Method called when a player joins the room the client is connected to.
        /// </summary>
        /// <param name="newPlayer"></param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("Player has entered the room: " + newPlayer.NickName);
            lobbyPlayer2Text.text = newPlayer.NickName;
            if (PhotonNetwork.PlayerList.Length == 2)
                StartGameButton.SetActive(true);
        }

        /// <summary>
        /// Method called when the client creates a room.
        /// </summary>
        public override void OnCreatedRoom()
        {
            lobbyPlayer1Text.text = PhotonNetwork.NickName;
            
        }

        /// <summary>
        /// Method called when the client leaves a room.
        /// </summary>
        public override void OnLeftRoom()
        {
            Debug.Log("PUNL OnLeftRoom() called by PUN. This client has left the room.");
            lobbyPlayer1Text.text = "Empty";
            lobbyPlayer2Text.text = "Empty";
            lobbyPanel.SetActive(false);
            controlPanel.SetActive(true);
        }

        /// <summary>
        /// Method called when a player leaves the room the client is connected to.
        /// </summary>
        /// <param name="otherPlayer"></param>
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("Player has left the room: " + otherPlayer.NickName);
            lobbyPlayer1Text.text = PhotonNetwork.NickName;
            lobbyPlayer2Text.text = "Empty";
        }


        #endregion

        #region Private Serializable Fields

        //[SerializeField] private byte maxPlayersPerRoom = 2;
        const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZ023456789";
        private string connectCode;

        #endregion


        #region Private Fields


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";
        string roomCode;
        

        [Tooltip("The UI panel to let the user enter name, connect and join")]
        [SerializeField] private GameObject controlPanel;
        [Tooltip("The UI panel to inform the user that the connection is in progress")]
        [SerializeField] private GameObject progressPanel;
        [Tooltip("The UI panel to display lobby information")]
        [SerializeField] private GameObject lobbyPanel;
        [SerializeField] private TMP_Text codeText;
        [SerializeField] private TMP_Text connectedStatusText;
        [SerializeField] private TMP_Text lobbyTitleText;
        [SerializeField] private TMP_Text lobbyPlayer1Text;
        [SerializeField] private TMP_Text lobbyPlayer2Text;
        [SerializeField] private GameObject invalidCodeText;
        [SerializeField] private GameObject StartGameButton;
        [SerializeField] private TMP_InputField codeInputField;

        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            if(PhotonNetwork.IsConnected)
            {
                connectedStatusText.text = "Connected to master server: " + PhotonNetwork.ServerAddress;
            } else
            {
                connectedStatusText.text = "Connection to the master server has failed. Do you have an active internet connection?";
            }
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            progressPanel.SetActive(false);
            controlPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            StartGameButton.SetActive(false);
            invalidCodeText.SetActive(false);
            codeInputField.characterLimit = 5;
            codeInputField.onValidateInput += delegate (string s, int i, char c) { return char.ToUpper(c); };
        }


        #endregion


        #region Public Methods


        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            for(int i = 0; i<5; i++)
            {
                connectCode += chars[Random.Range(0, chars.Length)];
            }
            progressPanel.SetActive(true);
            controlPanel.SetActive(false);
            Debug.Log("PUN: Connect() has been called, and a room will be created with code " + connectCode);
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = false;
            PhotonNetwork.JoinOrCreateRoom(connectCode, roomOptions, null);
        }

        /// <summary>
        /// Joins the room with the input room code.
        /// </summary>
        public void joinRoom()
        {
            PhotonNetwork.JoinRoom(roomCode);
            Debug.Log("PUN: joinRoom() has been called, and the client will join room ID " + roomCode);
        }

        /// <summary>
        /// Sets the room code to equal the current input in the input field.
        /// </summary>
        /// <param name="value"></param>
        public void SetRoomCode(string value)
        {
            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("RoomCode is null or empty");
                return;
            }
            roomCode = value;
        }

        /// <summary>
        /// Leaves the lobby.
        /// </summary>
        public void leaveLobby()
        {
            PhotonNetwork.LeaveRoom();
            connectCode = "";
        }

        public void startGame()
        {
            Debug.Log("Starting game.");
            PhotonNetwork.LoadLevel(1);
        }


        #endregion


    }
}
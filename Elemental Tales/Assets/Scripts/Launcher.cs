﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Com.Team12.ElementalTales
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region MonoBehaviourPunCallbacks Callbacks

        /// <summary>
        /// Method called when the client connects to the master servers (successful connection to online server host)
        /// </summary>
        //public override void OnConnectedToMaster()
        //{
        //    Debug.Log("PUN: OnConnectedToMaster() was called by PUN");
        //    RoomOptions roomOptions = new RoomOptions();
        //    roomOptions.IsVisible = false;
        //    PhotonNetwork.JoinOrCreateRoom(connectCode, roomOptions, null);
        //    Debug.Log("PUN: A room will be created with code " + connectCode);
        //}

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

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }

        #endregion

        #region Private Serializable Fields

        [SerializeField] private byte maxPlayersPerRoom = 2;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
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
        [SerializeField] private TMP_Text codeText;
        [SerializeField] private TMP_Text roomCodeText;
        [SerializeField] private TMP_Text connectedStatusText;


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
            for(int i = 0; i<8; i++)
            {
                connectCode += chars[Random.Range(0, chars.Length)];
            }
            codeText.text = connectCode;
            progressPanel.SetActive(true);
            controlPanel.SetActive(false);
            Debug.Log("PUN: Connect() has been called, and a room will be created with code " + connectCode);
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            //if (PhotonNetwork.IsConnected)
            //{
            RoomOptions roomOptions = new RoomOptions();
                roomOptions.IsVisible = false;
                PhotonNetwork.JoinOrCreateRoom(connectCode, roomOptions, null);
                
            //}
            //else
            //{
            //    // #Critical, we must first and foremost connect to Photon Online Server.
            //    PhotonNetwork.ConnectUsingSettings();
            //    PhotonNetwork.GameVersion = gameVersion;
            //    Debug.Log("PUN: Connect() has been called but the client is not connected. Connecting now.");
            //}
        }

        public void joinRoom(string roomCode)
        {
            if(roomCode !=  null)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.IsVisible = false;
                PhotonNetwork.JoinOrCreateRoom(roomCode, roomOptions, null);
                Debug.Log("PUN: joinRoom() has been called, and the client will join room ID " + roomCode);
            }
            Debug.Log("PUN: joinRoom() has been called but there is no input code.");
        }

        public void SetRoomCode(string value)
        {
            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("RoomCode is null or empty");
                return;
            }
            roomCode = value;
            roomCodeText.text = roomCode;
        }


        #endregion


    }
}
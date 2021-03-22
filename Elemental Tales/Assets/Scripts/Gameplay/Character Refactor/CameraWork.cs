using UnityEngine;
using Cinemachine;
using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
 *    
 *    Controls who the camera follows.
 */

public class CameraWork : MonoBehaviourPun
    {
        #region Private Fields



        [SerializeField] private bool followOnStart = false;
        private GameObject vcam;

        // maintain a flag internally to reconnect if target is lost or camera is switched
        //bool isFollowing;


        // Cache for camera offset
        Vector3 cameraOffset = Vector3.zero;


    #endregion

    #region Public Fields

    public SpriteRenderer playerSprite;

    #endregion


    #region MonoBehaviour Callbacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase
    /// </summary>
    void Start()
    {
        if (followOnStart)
        {
            OnStartFollowing();
        }

        try
        {
            if (photonView.IsMine)
                playerSprite.sortingOrder = 2000;
        } catch
        {
            Debug.LogWarning("CameraWork: photonView and playerSprite not yet initialised. Cause is unknown, but it does not affect gameplay. Ignore.");
            return;
        }
        
    }


        #endregion


        #region Public Methods


        /// <summary>
        /// Raises the start following event.
        /// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
        /// </summary>
        public void OnStartFollowing()
        {
            vcam = GameObject.Find("Virtual Camera");
            Follow();
        }


        #endregion


        #region Private Methods


        /// <summary>
        /// Follow the target smoothly.
        /// </summary>
        void Follow()
        {
            var virtcam = vcam.GetComponentsInChildren<CinemachineVirtualCamera>();
            
            virtcam[0].Follow = gameObject.transform;
            virtcam[0].LookAt = gameObject.transform;
        }
        #endregion
    }
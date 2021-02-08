using UnityEngine;
using Cinemachine;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
 *    
 *    Controls who the camera follows.
 */

public class CameraWork : MonoBehaviour
    {
        #region Private Fields



        [SerializeField] private bool followOnStart = false;
        private GameObject vcam;

        // maintain a flag internally to reconnect if target is lost or camera is switched
        //bool isFollowing;


        // Cache for camera offset
        Vector3 cameraOffset = Vector3.zero;


        #endregion


        #region MonoBehaviour Callbacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase
        /// </summary>
        void Start()
        {
            
            // Start following the target if wanted.
            if (followOnStart)
            {
                OnStartFollowing();
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
            //isFollowing = true;
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
            

            Debug.Log("Following " + gameObject.name);
            
            virtcam[0].Follow = gameObject.transform;
            virtcam[0].LookAt = gameObject.transform;
        }
        #endregion
    }
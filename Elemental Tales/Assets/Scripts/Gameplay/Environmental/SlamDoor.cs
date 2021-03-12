using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 2.2.1
 *    @version 1.0.0
 *    
 *    Setup to disable the gameobject when called by the pound ability
 */

public class SlamDoor : MonoBehaviourPun
{
    /// <summary>
    /// Triggers the RPC disabling the gameobject when called
    /// </summary>
    public void DestroyDoor()
    {
        photonView.RPC("DestroyDoorSlam", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Disables the gameobject for all players
    /// </summary>
    [PunRPC] private void DestroyDoorSlam()
    {
        gameObject.SetActive(false);
    }
}

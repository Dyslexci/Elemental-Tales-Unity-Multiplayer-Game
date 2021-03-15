using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 2.2.1
 *    @version 1.2.0
 *    
 *    Setup to disable the gameobject when called by the pound ability
 */

public class SlamDoor : MonoBehaviourPun
{
    public AudioSource destroySound;

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
        StartCoroutine(HandleDestroyedDoor());
    }

    /// <summary>
    /// Plays the sound and destroys the door.
    /// </summary>
    /// <returns></returns>
    IEnumerator HandleDestroyedDoor()
    {
        destroySound.Play(0);
        while(destroySound.isPlaying)
        {
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
    }
}

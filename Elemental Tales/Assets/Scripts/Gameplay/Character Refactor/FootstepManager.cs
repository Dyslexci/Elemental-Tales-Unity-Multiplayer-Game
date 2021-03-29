using Photon.Pun;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 2.2.0
 *    @version 1.0.0
 *
 *    Causes the footsteps to be triggered on the network on the keyframe of the walking animation.
 */

public class FootstepManager : MonoBehaviourPun
{
    public AudioSource[] footstepsAudio;
    public CharacterControllerRaycast controller;

    /// <summary>
    /// Plays footstep sounds if the player is on the floor.
    /// </summary>
    public void PlayFootsteps()
    {
        if (!controller.collisions.below)
            return;

        photonView.RPC("TriggerFootstep", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Plays the footstep sound for all players.
    /// </summary>
    [PunRPC]
    private void TriggerFootstep()
    {
        footstepsAudio[Random.Range(0, footstepsAudio.Length)].Play(0);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FootstepManager : MonoBehaviourPun
{
    public AudioSource[] footstepsAudio;

    public void PlayFootsteps()
    {
        photonView.RPC("TriggerFootstep", RpcTarget.AllBuffered);
    }

    [PunRPC] private void TriggerFootstep()
    {
        footstepsAudio[Random.Range(0, footstepsAudio.Length)].Play(0);
    }
}

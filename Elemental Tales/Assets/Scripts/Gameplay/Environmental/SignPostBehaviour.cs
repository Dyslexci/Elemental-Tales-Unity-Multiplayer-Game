using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SignPostBehaviour : MonoBehaviour
{
    public CanvasGroup signs;
    [SerializeField] Transform pos;
    [SerializeField] float radius = 1.5f;
    [SerializeField] private LayerMask layer;

    private bool playerPresent = false;
    private bool playerWasPresent;

    /// <summary>
    /// Sets the signs to start the game invisible
    /// </summary>
    private void Start()
    {
        signs.alpha = 0;
    }

    /// <summary>
    /// Checks for player presence and whether to fade in or out the signs
    /// </summary>
    private void FixedUpdate()
    {
        playerWasPresent = playerPresent;
        playerPresent = false;

        checkPresent();

        if (playerPresent && !playerWasPresent)
        {
            StopCoroutine(FadeOutSigns());
            StartCoroutine(FadeInSigns());
        }

        if(!playerPresent && playerWasPresent)
        {
            StopCoroutine(FadeInSigns());
            StartCoroutine(FadeOutSigns());
        }
    }

    /// <summary>
    /// Checks if the local player is within a defined radius of a defined position.
    /// </summary>
    private void checkPresent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.position, radius, layer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                if (colliders[i].gameObject.GetPhotonView().IsMine)
                {
                    playerPresent = true;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Fades in the signs
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeInSigns()
    {
        GameObject.Find("Game Manager").GetComponent<GameMaster>().signpostEnterRange.Play(0);
        while (signs.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            signs.alpha += .02f;
        }
    }

    /// <summary>
    /// Fades out the signs
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutSigns()
    {
        GameObject.Find("Game Manager").GetComponent<GameMaster>().signpostExitRange.Play(0);
        while (signs.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            signs.alpha -= .01f;
        }
    }
}

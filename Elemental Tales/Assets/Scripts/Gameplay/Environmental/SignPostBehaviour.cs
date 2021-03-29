using System.Collections;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 2.1.0
 *    @version 1.0.3
 *
 *    Displays signpost text in the world while the player is nearby.
 */

public class SignPostBehaviour : CheckPresentController
{
    public CanvasGroup signs;
    [SerializeField] private Transform pos;
    [SerializeField] private float radius = 1.5f;
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
        playerPresent = CheckPresentCircle(pos, radius, layer);

        FadeSignpostFromPlayerPresence();
    }

    private void FadeSignpostFromPlayerPresence()
    {
        if (playerPresent && !playerWasPresent)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInSigns());
        }

        if (!playerPresent && playerWasPresent)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutSigns());
        }
    }

    /// <summary>
    /// Fades in the signs
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInSigns()
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
    private IEnumerator FadeOutSigns()
    {
        GameObject.Find("Game Manager").GetComponent<GameMaster>().signpostExitRange.Play(0);
        while (signs.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            signs.alpha -= .01f;
        }
    }
}
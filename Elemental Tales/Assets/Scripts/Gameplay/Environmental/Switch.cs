using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 2.0.2
 *    
 *    Manages players interacting with physical switch objects in the world.
 */

public class Switch : MonoBehaviourPun
{
    [SerializeField] Sprite crankDown;
    [SerializeField] Sprite crankUp;
    [SerializeField] Transform pos;
    [SerializeField] float radius = 1.5f;
    [SerializeField] private LayerMask layer;
    private bool isOn = false;
    public bool pressedSuccessfully = false;
    private bool playerPresent = false;
    bool otherPlayerPresent;
    private bool playerWasPresent;
    GameObject playerCollider = null;

    TMP_Text hintText;
    GameObject hintHolder;
    CanvasGroup panel;
    Image hintImage;
    bool isDisplayingHint;

    /// <summary>
    /// Initialises switch values.
    /// </summary>
    void Start()
    { 
        gameObject.GetComponent<SpriteRenderer>().sprite = crankDown;

        hintText = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintText();
        hintHolder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().GetHintHolder();
        hintImage = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintContainer();
        panel = hintHolder.GetComponentInChildren<CanvasGroup>();
    }

    /// <summary>
    /// Checks for player presence and whether the player is pulling the switch. Triggers an RPC if the player is interacting with the switch.
    /// </summary>
    private void FixedUpdate()
    {
        if(pressedSuccessfully)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = crankUp;
            return;
        }
        playerWasPresent = playerPresent;
        playerPresent = false;
        otherPlayerPresent = false;

        checkPresent();

        if (playerPresent && Input.GetButton("Interact"))
        {
            if (isOn == true)
                return;
            photonView.RPC("setLeverOn", RpcTarget.AllBuffered);
            GameObject.Find("Game Manager").GetComponent<GameMaster>().switchPull.Play(0);
        } else if(playerPresent && !otherPlayerPresent)
        {
            if (isOn == false)
                return;
            photonView.RPC("setLeverOff", RpcTarget.AllBuffered);
            GameObject.Find("Game Manager").GetComponent<GameMaster>().switchPull.Play(0);
        }
    }

    /// <summary>
    /// Refactored by Adnan
    /// Checks for the players presence based off Physics2D collider circles and displays the hint if the player has entered the switch collider.
    /// </summary>
    private void checkPresent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.position, radius, layer);

        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Player")
            {
                if (c.gameObject.GetPhotonView().IsMine)
                {
                    playerCollider = c.gameObject;
                    if (isDisplayingHint == false && playerWasPresent == false)
                    {
                        isDisplayingHint = true;
                        GameObject.Find("Game Manager").GetComponent<GameMaster>().hintSound.Play(0);
                        StartCoroutine(WaitHideHint());
                    }
                    playerPresent = true;
                    return;
                }
                if (!c.gameObject.GetPhotonView().IsMine)
                {
                    otherPlayerPresent = true;
                }
            }
            else
            {
                playerCollider = null;
            }
        }
    }

    /// <summary>
    /// Draws a cisual representation of the switch collider in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(pos.position, radius);
    }

    /// <summary>
    /// Returns the current state of the lever.
    /// </summary>
    /// <returns></returns>
    public bool getLeverState()
    {
        return isOn;
    }

    /// <summary>
    /// Sets the state of the lever to successfully pressed once the final condition of the interaction is filled; e.g. the door has opened.
    /// </summary>
    public void setPressedSuccessfully()
    {
        pressedSuccessfully = true;
        isOn = false;
    }

    /// <summary>
    /// Sends an RPC to other player triggering the lever to its on state.
    /// </summary>
    [PunRPC] private void setLeverOn()
    {
        Debug.Log("PUN: setLeverOn() has been called.");
        gameObject.GetComponent<SpriteRenderer>().sprite = crankUp;
        isOn = true;
    }

    /// <summary>
    /// Sends an RPC to the other player triggering the lever to its off state.
    /// </summary>
    [PunRPC] private void setLeverOff()
    {
        Debug.Log("PUN: setLeverOff() has been called.");
        gameObject.GetComponent<SpriteRenderer>().sprite = crankDown;
        isOn = false;
    }

    /// <summary>
    /// Coroutine displaying the hint to the player.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitHideHint()
    {
        hintHolder.SetActive(true);
        StartCoroutine("JumpInHintHolder");
        hintText.text = "<color=#ffffff>Hold E to <color=#ffeb04>grab and switch <color=#ffffff>levers!";
        yield return new WaitForSeconds(2);
        StartCoroutine("FadeHintHolder");
    }

    /// <summary>
    /// Coroutine making the hint jump into place.
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpInHintHolder()
    {
        hintImage.transform.localScale = new Vector3(5, 5, 5);

        while (hintImage.transform.localScale.x > 1)
        {
            yield return new WaitForFixedUpdate();
            hintImage.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        }
        hintImage.transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Coroutine making the hint fade out after it has been on screen enough time.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeHintHolder()
    {
        while (panel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            panel.alpha -= 0.05f;
        }

        hintHolder.SetActive(false);
        panel.alpha = 1;
        isDisplayingHint = false;
    }
}
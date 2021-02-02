using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

/*
 * @author Afoke Chizea
 * the script changes the animation of the
 */
public class Switch : MonoBehaviourPun
{
    [SerializeField] Sprite crankDown;
    [SerializeField] Sprite crankUp;
    [SerializeField] Transform pos;
    [SerializeField] float radius = 1.5f;
    [SerializeField] private LayerMask layer;
    private bool isOn = false;
    private bool pressedSuccessfully = false;
    private bool playerPresent = false;
    bool otherPlayerPresent;
    private bool playerWasPresent;
    GameObject playerCollider = null;

    TMP_Text hintText;
    GameObject hintHolder;
    CanvasGroup panel;
    Image hintImage;
    bool isDisplayingHint;

    public bool debug;

    void Start()
    { 
        gameObject.GetComponent<SpriteRenderer>().sprite = crankDown;

        hintText = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintText();
        hintHolder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().GetHintHolder();
        hintImage = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintContainer();
        panel = hintHolder.GetComponentInChildren<CanvasGroup>();
    }

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
        if(debug)
        {
            print("Checked present, result is: " + playerPresent);
        }

        if (playerPresent && Input.GetButton("Interact"))
        {
            if (isOn == true)
                return;
            photonView.RPC("setLeverOn", RpcTarget.AllBuffered);
        } else if(playerPresent && !otherPlayerPresent)
        {
            if (isOn == false)
                return;
            photonView.RPC("setLeverOff", RpcTarget.AllBuffered);
        }
    }

    private void checkPresent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.position, radius, layer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                if (colliders[i].gameObject.GetPhotonView().IsMine)
                {
                    playerCollider = colliders[i].gameObject;
                    if (isDisplayingHint == false && playerWasPresent == false)
                    {
                        isDisplayingHint = true;
                        StartCoroutine("WaitHideHint");
                    }
                    playerPresent = true;
                    return;
                } 
                if (!colliders[i].gameObject.GetPhotonView().IsMine)
                {
                    otherPlayerPresent = true;
                }
            }
            else
            {
                //playerPresent = false;
                playerCollider = null;
            }

            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(pos.position, radius);
    }

    public bool getLeverState()
    {
        return isOn;
    }

    public void setPressedSuccessfully()
    {
        pressedSuccessfully = true;
    }

    [PunRPC] private void setLeverOn()
    {
        Debug.Log("PUN: setLeverOn() has been called.");
        gameObject.GetComponent<SpriteRenderer>().sprite = crankUp;
        isOn = true;
    }

    [PunRPC] private void setLeverOff()
    {
        Debug.Log("PUN: setLeverOff() has been called.");
        gameObject.GetComponent<SpriteRenderer>().sprite = crankDown;
        isOn = false;
    }

    IEnumerator WaitHideHint()
    {
        GameObject.Find("Game Manager").GetComponent<GameMaster>().hintSound.Play(0);
        hintHolder.SetActive(true);
        StartCoroutine("JumpInHintHolder");
        hintText.text = "<color=#ffffff>Press E to <color=#ffeb04> grab and switch <color=#ffffff>levers!";
        yield return new WaitForSeconds(2);
        StartCoroutine("FadeHintHolder");
    }

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
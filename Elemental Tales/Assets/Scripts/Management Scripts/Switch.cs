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
    private bool playerWasPresent;
    GameObject playerCollider = null;

    TMP_Text hintText;
    GameObject hintHolder;
    CanvasGroup panel;
    Image hintImage;
    bool isDisplayingHint;

    void Start()
    { 
        gameObject.GetComponent<SpriteRenderer>().sprite = crankDown;

        hintText = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintText();
        hintHolder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().GetHintHolder();
        hintImage = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintContainer();
        panel = hintHolder.GetComponentInChildren<CanvasGroup>();
    }

    private void Update()
    {
        if(pressedSuccessfully)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = crankUp;
            return;
        }
        playerWasPresent = playerPresent;

        if(!playerWasPresent && playerPresent && !isDisplayingHint)
        {
            isDisplayingHint = true;
            StartCoroutine("WaitHideHint");
        }

        checkPresent();

        if (playerCollider != null)
            if(!playerCollider.GetPhotonView().IsMine)
                return;

        if (playerPresent && Input.GetButton("Interact"))
        {
            if (isOn == true)
                return;
            photonView.RPC("setLeverOn", RpcTarget.AllBuffered);
        } else if(playerPresent)
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
                if (playerPresent == true)
                    return;
                
                playerPresent = true;
                playerCollider = colliders[i].gameObject;
            }
            else
            {
                if (playerPresent == false)
                    return;
                playerPresent = false;
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
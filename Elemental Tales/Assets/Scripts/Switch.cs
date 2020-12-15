using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

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
    private bool isOn = false;
    private bool pressedSuccessfully = false;
    private bool playerPresent = false;



    void Start()
    { 
        gameObject.GetComponent<SpriteRenderer>().sprite = crankDown;
    }

    private void Update()
    {
        if(pressedSuccessfully)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = crankUp;
            return;
        }

        checkPresent();

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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                if (playerPresent == true)
                    return;
                Debug.Log("Player has entered the region");
                playerPresent = true;
            }
            else
            {
                if (playerPresent == false)
                    return;
                Debug.Log("Player has left the region");
                playerPresent = false;
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
}
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEditor.PlayerSettings;
/*
 * @author Afoke Chizea
 * the script changes the animation of the
 */
public class Switch : MonoBehaviour
{
    [SerializeField] GameObject crankDown;
    [SerializeField] GameObject crankUp;
    private bool isOn = false;
    private bool pressedSuccessfully = false;
    private bool playerPresent = false;



    void Start()
    { 
        gameObject.GetComponent<SpriteRenderer>().sprite = crankDown.GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        if(pressedSuccessfully)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = crankUp.GetComponent<SpriteRenderer>().sprite;
            return;
        }

        if(playerPresent && Input.GetButton("Interact"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = crankUp.GetComponent<SpriteRenderer>().sprite;
            isOn = true;
        } else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = crankDown.GetComponent<SpriteRenderer>().sprite;
            isOn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        playerPresent = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerPresent = false;
    }

    public bool getLeverState()
    {
        return isOn;
    }

    public void setPressedSuccessfully()
    {
        pressedSuccessfully = true;
    }
}
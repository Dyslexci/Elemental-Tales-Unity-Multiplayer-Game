using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/** 
 *    @author David Chizea, @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.1
 *    
 *    Script allows levers to be toggled.
 */

public class Switch : MonoBehaviour
{
   [SerializeField] private GameObject crankDown;
   [SerializeField] private GameObject crankUp;
    private bool isOn;
    private bool playerInArea;
    private bool alreadyPressed;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = crankDown.GetComponent<SpriteRenderer>().sprite;
        isOn = false;
        playerInArea = false;
        alreadyPressed = false;
    }

    private void Update()
    {
        if (alreadyPressed)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = crankUp.GetComponent<SpriteRenderer>().sprite;
            return;
        }
            
        if (playerInArea && Input.GetButton("Interact"))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = crankUp.GetComponent<SpriteRenderer>().sprite;
            isOn = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = crankDown.GetComponent<SpriteRenderer>().sprite;
            isOn = false;
        }
            
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        playerInArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        playerInArea = false;
    }

    public bool getLeverState()
    {
        return isOn;
    }

    public void setPressedSuccessfully()
    {
        alreadyPressed = true;
    }
}
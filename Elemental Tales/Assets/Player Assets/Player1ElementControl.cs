using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    This script sets the player 1 element, changes the hud accordingly, and changes the player controller element variable.
 */

public class Player1ElementControl : MonoBehaviour
{
    private string currentElement;
    [SerializeField] private ElementOrb elementOrb;
    [SerializeField] private CharacterControllerPlayer1 controller;

    void Start()
    {
        currentElement = "Air";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            currentElement = "Air";
            controller.changeElement("Air");
            elementOrb.setElement("Air");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentElement = "Earth";
            controller.changeElement("Earth");
            elementOrb.setElement("Earth");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            currentElement = "Water";
            controller.changeElement("Water");
            elementOrb.setElement("Water");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentElement = "Fire";
            controller.changeElement("Fire");
            elementOrb.setElement("Fire");
        }
    }
}

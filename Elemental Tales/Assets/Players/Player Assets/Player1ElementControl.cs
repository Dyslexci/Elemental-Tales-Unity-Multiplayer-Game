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
    private bool hasAir;
    private bool hasFire;
    private bool hasEarth;
    private bool hasWater;

    private void Start()
    {
        hasAir = false;
        hasFire = false;
        hasEarth = false;
        hasWater = false;
        currentElement = "Empty";
        controller.changeElement("Empty");
        elementOrb.setElement("Empty");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasAir)
        {
            currentElement = "Air";
            controller.changeElement("Air");
            elementOrb.setElement("Air");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasEarth)
        {
            currentElement = "Earth";
            controller.changeElement("Earth");
            elementOrb.setElement("Earth");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasWater)
        {
            currentElement = "Water";
            controller.changeElement("Water");
            elementOrb.setElement("Water");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && hasFire)
        {
            currentElement = "Fire";
            controller.changeElement("Fire");
            elementOrb.setElement("Fire");
        }
    }

    private string getElement()
    {
        return currentElement;
    }

    public void addElement(string element)
    {
        if(element.Equals("Air"))
        {
            Debug.Log("Added air element");
            hasAir = true;
        } else if(element.Equals("Earth"))
        {
            Debug.Log("Added earth element");
            hasEarth = true;
        } else if(element.Equals("Water"))
        {
            Debug.Log("Added water element");
            hasWater = true;
        } else if(element.Equals("Fire"))
        {
            Debug.Log("Added fire element");
            hasFire = true;
        }
    }
}

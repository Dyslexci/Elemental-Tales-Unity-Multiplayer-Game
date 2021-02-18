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

[RequireComponent(typeof(ElementOrb))]
public class ElementController : MonoBehaviour
{
    string currentElement;
    ElementOrb elementOrb;
    StatController controller;
    bool hasAir;
    bool hasFire;
    bool hasEarth;
    bool hasWater;

    private void Start()
    {
        elementOrb = GetComponent<ElementOrb>();
        controller = GetComponent<StatController>();
        hasAir = false;
        hasFire = false;
        hasEarth = false;
        hasWater = false;
        currentElement = "Empty";
        controller.currentElement = "Empty";
        elementOrb.setElement("Empty");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasAir)
        {
            currentElement = "Air";
            controller.currentElement = "Air";
            elementOrb.setElement("Air");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasEarth)
        {
            currentElement = "Earth";
            controller.currentElement = "Earth";
            elementOrb.setElement("Earth");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasWater)
        {
            currentElement = "Water";
            controller.currentElement = "Water";
            elementOrb.setElement("Water");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && hasFire)
        {
            currentElement = "Fire";
            controller.currentElement = "Fire";
            elementOrb.setElement("Fire");
        }
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentElement = "Empty";
            controller.currentElement = "Empty";
            elementOrb.setElement("Empty");
        }
    }

    public string getElement()
    {
        return currentElement;
    }

    public void addElement(string element)
    {
        if(element.Equals("Air"))
        {
            hasAir = true;
        } else if(element.Equals("Earth"))
        {
            hasEarth = true;
        } else if(element.Equals("Water"))
        {
            hasWater = true;
        } else if(element.Equals("Fire"))
        {
            hasFire = true;
        }
    }
}

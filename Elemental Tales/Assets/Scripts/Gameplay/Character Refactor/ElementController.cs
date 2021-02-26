using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.0.0
 *    
 *    This script sets the player 1 element, changes the hud accordingly, and changes the player controller element variable.
 */

[RequireComponent(typeof(ElementOrb))]
public class ElementController : MonoBehaviour
{
    string currentElement;
    ElementOrb elementOrb;
    StatController controller;
    public bool hasAir;
    public bool hasFire;
    public bool hasEarth;
    public bool hasWater;

    /// <summary>
    /// Sets the start conditions where possible
    /// </summary>
    private void Start()
    {
        elementOrb = GetComponent<ElementOrb>();
        controller = GetComponent<StatController>();
        hasAir = false;
        hasFire = false;
        hasEarth = false;
        hasWater = false;
        StartCoroutine(delaySetElements());
    }

    /// <summary>
    /// Delays setting the start conditions for the elements, as sometimes the player spawning is delayed by a frame
    /// </summary>
    /// <returns></returns>
    IEnumerator delaySetElements()
    {
        yield return new WaitForSeconds(1);
        currentElement = "Empty";
        controller.currentElement = "Empty";
        elementOrb.setElement("Empty");
    }

    /// <summary>
    /// Allows the player to choose the active element
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasAir)
        {
            currentElement = "Air";
            controller.currentElement = "Air";
            elementOrb.setElement("Air");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasEarth)
        {
            currentElement = "Earth";
            controller.currentElement = "Earth";
            elementOrb.setElement("Earth");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasWater)
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

    /// <summary>
    /// Returns the current chosen element
    /// </summary>
    /// <returns></returns>
    public string getElement()
    {
        return currentElement;
    }

    /// <summary>
    /// Adds a specific element to the available elements for this player
    /// </summary>
    /// <param name="element"></param>
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

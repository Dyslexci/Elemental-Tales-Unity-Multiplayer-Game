using System.Collections;
using UnityEngine;

/**
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 2.0.1
 *
 *    This script sets the player 1 element, changes the hud accordingly, and changes the player controller element variable.
 */

[RequireComponent(typeof(ElementOrb))]
public class ElementController : MonoBehaviour
{
    private string currentElement;
    private ElementOrb elementOrb;
    private StatController controller;
    public bool hasAir;
    public bool hasFire;
    public bool hasEarth;
    public bool hasWater;
    private AudioSource[] elementChangeAudio;

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
    private IEnumerator delaySetElements()
    {
        yield return new WaitForSeconds(1);
        currentElement = "Empty";
        controller.currentElement = "Empty";
        elementOrb.setElement("Empty");
        elementChangeAudio = GameObject.Find("Game Manager").GetComponent<GameMaster>().elementChangeAudio;
    }

    /// <summary>
    /// Refactored by Adnan
    /// Allows the player to choose the active element
    /// </summary>
    private void Update()
    {
        CheckForElementChange();
    }

    private void CheckForElementChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasAir)
        {
            changeElement("Air");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasEarth)
        {
            changeElement("Earth");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasWater)
        {
            changeElement("Water");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && hasFire)
        {
            changeElement("Fire");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            changeElement("Empty");
        }
    }

    /// <summary>
    /// Refactored by Adnan
    /// changes element based on param
    /// </summary>
    /// <param name="e"></param>
    private void changeElement(string e)
    {
        elementChangeAudio[Random.Range(0, elementChangeAudio.Length)].Play(0);
        currentElement = e;
        controller.currentElement = e;
        elementOrb.setElement(e);
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
        if (element.Equals("Air"))
        {
            hasAir = true;
        }
        else if (element.Equals("Earth"))
        {
            hasEarth = true;
        }
        else if (element.Equals("Water"))
        {
            hasWater = true;
        }
        else if (element.Equals("Fire"))
        {
            hasFire = true;
        }
    }
}
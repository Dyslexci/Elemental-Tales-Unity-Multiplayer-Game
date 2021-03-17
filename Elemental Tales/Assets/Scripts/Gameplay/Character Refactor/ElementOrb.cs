using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.0.0
 *    
 *    This script controls the visual element orb on the HUD.
 */

[System.Serializable]
public class ElementOrb : MonoBehaviour
{
    private string currentElement;
    Image elementOrb;

    [SerializeField] private Sprite EarthOrb;
    [SerializeField] private Sprite WaterOrb;
    [SerializeField] private Sprite AirOrb;
    [SerializeField] private Sprite FireOrb;
    [SerializeField] private Sprite emptyOrb;

    /// <summary>
    /// Initialises the HUD object this object needs to interact with.
    /// </summary>
    private void Start()
    {
        elementOrb = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getElementOrb();
    }

    /// <summary>
    /// Sets the current element and updates the HUD object respectively.
    /// </summary>
    /// <param name="element"></param>
    public void setElement(string element)
    {
        currentElement = element;
        if(element.Equals("Earth"))
        {
            elementOrb.sprite = EarthOrb;
        } else if (element.Equals("Water"))
        {
            elementOrb.sprite = WaterOrb;
        } else if (element.Equals("Air"))
        {
            elementOrb.sprite = AirOrb;
        } else if (element.Equals("Fire"))
        {
            elementOrb.sprite = FireOrb;
        } else
        {
            elementOrb.sprite = emptyOrb;
        }
    }
}

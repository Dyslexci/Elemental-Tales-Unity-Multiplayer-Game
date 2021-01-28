﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
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

    private void Start()
    {
        elementOrb = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getElementOrb();
    }

    public void setElement(string element)
    {
        currentElement = element;
        if(element.Equals("Earth"))
        {
            elementOrb.sprite = EarthOrb;
            print("Changed to earth");
        } else if (element.Equals("Water"))
        {
            elementOrb.sprite = WaterOrb;
            print("Changed to water");
        } else if (element.Equals("Air"))
        {
            elementOrb.sprite = AirOrb;
            print("Changed to air");
        } else if (element.Equals("Fire"))
        {
            elementOrb.sprite = FireOrb;
            print("Changed to fire");
        } else
        {
            elementOrb.sprite = emptyOrb;
            print("Changed to empty");
        }
    }
}
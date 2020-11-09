using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ElementOrb : MonoBehaviour
{
    private string currentElement;
    [SerializeField] private Image elementOrb;

    [SerializeField] private Sprite EarthOrb;
    [SerializeField] private Sprite WaterOrb;
    [SerializeField] private Sprite AirOrb;
    [SerializeField] private Sprite FireOrb;

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
        } else
        {
            elementOrb.sprite = FireOrb;
            print("Changed to fire");
        }
    }
}

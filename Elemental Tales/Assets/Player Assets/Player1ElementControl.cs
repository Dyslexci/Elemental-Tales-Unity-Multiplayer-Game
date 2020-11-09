using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1ElementControl : MonoBehaviour
{
    private string currentElement;
    [SerializeField] private ElementOrb elementOrb;
    [SerializeField] private CharacterControllerPlayer1 controller;
    // Start is called before the first frame update
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2ElementControl : MonoBehaviour
{
    private string currentElement;
    //[SerializeField] private ElementOrb elementOrb;
    [SerializeField] private CharacterControllerPlayer1 controller;
    // Start is called before the first frame update
    void Start()
    {
        currentElement = "Air";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            print("Player 2 element is now Air");
            currentElement = "Air";
            controller.changeElement("Air");
            //elementOrb.setElement("Air");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            print("Player 2 element is now Earth");
            currentElement = "Earth";
            controller.changeElement("Earth");
            //elementOrb.setElement("Earth");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            print("Player 2 element is now Water");
            currentElement = "Water";
            controller.changeElement("Water");
            //elementOrb.setElement("Water");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            print("Player 2 element is now Fire");
            currentElement = "Fire";
            controller.changeElement("Fire");
            //elementOrb.setElement("Fire");
        }
    }
}

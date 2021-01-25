using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterControllerRaycast))]
public class PlayerInputs : MonoBehaviour
{
    CharacterControllerRaycast controller;

    void Start()
    {
        controller = GetComponent<CharacterControllerRaycast>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

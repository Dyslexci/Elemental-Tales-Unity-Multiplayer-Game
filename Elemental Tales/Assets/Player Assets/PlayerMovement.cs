﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    This script provides raw inputs for relay to the character controller, as well as controls animations based off of inputs.
 */

public class PlayerMovement : MonoBehaviour
{
    public CharacterControllerPlayer1 controller;
    //public Animator animator;

    private float runSpeed = 40f;
    private float HorizontalMove = 0f;
    private Boolean jump = false;
    private Boolean crouch = false;

    private void Update()
    {
        HorizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //animator.SetFloat("Speed", Mathf.Abs(HorizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            //animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    public void onLanding()
    {
        //animator.SetBool("isJumping", false);
    }

    public void onCrouching(bool isCrouching)
    {
        //animator.SetBool("isCrouching", isCrouching);
    }

    private void FixedUpdate()
    {
        controller.Move(HorizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
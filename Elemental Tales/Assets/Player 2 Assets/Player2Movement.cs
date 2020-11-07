using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public CharacterControllerPlayer1 controller;
    //public Animator animator;

    [SerializeField] private float runSpeed = 60f;
    private float HorizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;

    private void Update()
    {
        HorizontalMove = Input.GetAxisRaw("Horizontal2") * runSpeed;

        //animator.SetFloat("Speed", Mathf.Abs(HorizontalMove));

        if (Input.GetButtonDown("Jump2"))
        {
            jump = true;
            //animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonDown("Crouch2"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch2"))
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

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    This script will provide controls for the character of player 1. It will smooth movement, allow jumping and crouching, abilities, and store health etc.
 */

public class CharacterControllerPlayer2 : MonoBehaviour
{
    [SerializeField] private ElementOrb elementOrb;
    [SerializeField] private Health health;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int maxMana = 3;
    [SerializeField] private Transform mGroundCheck;
    [SerializeField] private Transform mCeilingCheck;
    [SerializeField] private Collider2D mCrouchColliderToggle;
    [SerializeField] private float m_JumpForce = 650f;
    [Range(0, 1)] [SerializeField] private float mCrouchSpeed = 0.4f;
    [Range(0, 1)] [SerializeField] private float mMovementSmoothing = 0.05f;
    [SerializeField] private LayerMask mGroundIdentifier;

    [SerializeField] private Rigidbody2D mRigidBody2D;
    private int currentCollectibles1;
    private bool mFacingRight = true;
    private bool mGrounded;
    private const float mGroundedRadius = 0.05f;
    private const float mCeilingRadius = 0.2f;
    private Vector3 mVelocity = Vector3.zero;
    private Boolean HasDoubleJump = true;
    private float JumpForce;
    private bool isStomp = false;

    public int currentHealth;
    public int currentMana;
    private string currentElement;
    public int numOfHealthContainers;
    public int numOfManaContainers;
    public UnityEngine.UI.Image[] heartContainers;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent2;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent2;
    private bool mWasCrouching = false;

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentCollectibles1 = 0;
        changeElement("Air");
    }

    private void Awake()
    {
        mRigidBody2D = GetComponent<Rigidbody2D>();
        if (OnLandEvent2 == null)
            OnLandEvent2 = new UnityEvent();
        if (OnCrouchEvent2 == null)
            OnCrouchEvent2 = new BoolEvent();
    }

    private void Update()
    {
        bool wasTouchingGround = mGrounded;
        mGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mGroundCheck.position, mGroundedRadius, mGroundIdentifier);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                mGrounded = true;
                HasDoubleJump = true;
                isStomp = false;
                if (!wasTouchingGround)
                    OnLandEvent2.Invoke();
            }
        }

        if (!mGrounded && mRigidBody2D.velocity.y < 0)
        {
            mRigidBody2D.gravityScale = (float)2;
        }
        else
        {
            mRigidBody2D.gravityScale = 3;
        }
    }

    public void DamageHealth(int damage)
    {
        print("Taken damage");
        health.takeDamage(damage);
    }

    public void stompAttack()
    {
        if(currentElement.Equals("Earth"))
        {
            print("Stomping");
            mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0);
            mRigidBody2D.AddRelativeForce(new Vector2(0f, -900));
            isStomp = true;
        }
    }

    public bool isStomping()
    {
        return isStomp;
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (!crouch)
        {
            if (Physics2D.OverlapCircle(mCeilingCheck.position, mCeilingRadius, mGroundIdentifier))
                crouch = true;
        }

        if (crouch)
        {
            if (!mWasCrouching)
            {
                mWasCrouching = true;
                OnCrouchEvent2.Invoke(true);
            }
            move *= mCrouchSpeed;
            if (mCrouchColliderToggle != null)
                mCrouchColliderToggle.enabled = false;
        } else
        {
            if (mCrouchColliderToggle != null)
                mCrouchColliderToggle.enabled = true;
            if (mWasCrouching)
            {
                mWasCrouching = false;
                OnCrouchEvent2.Invoke(false);
            }
            
        }

        Vector3 targetVelocity = new Vector2(move * 10f, mRigidBody2D.velocity.y);
        mRigidBody2D.velocity = Vector3.SmoothDamp(mRigidBody2D.velocity, targetVelocity, ref mVelocity, mMovementSmoothing);
        if (move > 0 && !mFacingRight)
            Flip();
        else if (move < 0 && mFacingRight)
            Flip();

        if(!mGrounded && jump && HasDoubleJump && currentElement.Equals("Air"))
        {
            mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0);
            mRigidBody2D.AddRelativeForce(new Vector2(0f, 400));
            HasDoubleJump = false;
        }
        if (mGrounded && jump)
        {
            mGrounded = false;
            mRigidBody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
        

    }

    public bool checkIsGrounded()
    {
        return mGrounded;
    }

    private void Flip()
    {
        mFacingRight = !mFacingRight;
        Vector3 rotateScale = transform.localScale;
        rotateScale.x *= -1;
        transform.localScale = rotateScale;
    }

    public void changeHealth(int healthModifier)
    {
        if ((currentHealth + healthModifier < maxHealth) && (currentHealth + healthModifier >= 0))
            currentHealth += healthModifier;
    }

    public void changeMana(int manaModifier)
    {
        if ((currentMana + manaModifier < maxMana) && (currentHealth + manaModifier >= 0))
            currentMana += manaModifier;
    }

    public void changeElement(string newElement)
    {
        currentElement = newElement;
        //elementOrb.setElement(newElement);
    }

    public string getElement()
    {
        return currentElement;
    }

    public void addCollectible1()
    {
        currentCollectibles1++;
    }
}
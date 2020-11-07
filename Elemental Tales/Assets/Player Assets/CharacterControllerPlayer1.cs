using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    This script will provide controls for the character of player 1. It will smooth movement, allow jumping and crouching, abilities, and store health etc.
 */

public class CharacterControllerPlayer1 : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int maxMana = 3;
    [SerializeField] private Transform mGroundCheck;
    [SerializeField] private Transform mCeilingCheck;
    [SerializeField] private Collider2D mCrouchColliderToggle;
    [SerializeField] private float mJumpForce = 500f;
    [Range(0, 1)] [SerializeField] private float mCrouchSpeed = 0.4f;
    [Range(0, 1)] [SerializeField] private float mMovementSmoothing = 0.05f;
    [SerializeField] private LayerMask mGroundIdentifier;

    private Rigidbody2D mRigidBody2D;
    private int currentHealth;
    private int currentMana;
    private string currentElement;
    private int currentCollectibles1;
    private bool mFacingRight = true;
    private bool mGrounded;
    private const float mGroundedRadius = 0.1f;
    private const float mCeilingRadius = 0.2f;
    private Vector3 mVelocity = Vector3.zero;
    private Boolean HasDoubleJump = true;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    public BoolEvent OnCrouchEvent;
    private bool mWasCrouching = false;

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentCollectibles1 = 0;
        currentElement = "earth";
    }

    private void Awake()
    {
        mRigidBody2D = GetComponent<Rigidbody2D>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void Update()
    {
        mGrounded = false;
        bool wasTouchingGround = mGrounded;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mGroundCheck.position, mGroundedRadius, mGroundIdentifier);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                mGrounded = true;
                HasDoubleJump = true;
                if (!wasTouchingGround)
                    OnLandEvent.Invoke();
            }
        }
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
                OnCrouchEvent.Invoke(true);
            }
            move *= mCrouchSpeed;
            if (mCrouchColliderToggle != null)
                mCrouchColliderToggle.enabled = false;
        } else
        {
            if (!mWasCrouching)
            {
                mWasCrouching = false;
                OnCrouchEvent.Invoke(false);
            }
            if (mCrouchColliderToggle != null)
                mCrouchColliderToggle.enabled = true;
        }

        Vector3 targetVelocity = new Vector2(move * 10f, mRigidBody2D.velocity.y);
        mRigidBody2D.velocity = Vector3.SmoothDamp(mRigidBody2D.velocity, targetVelocity, ref mVelocity, mMovementSmoothing);
        if (move > 0 && !mFacingRight)
            Flip();
        else if (move < 0 && mFacingRight)
            Flip();

        if(!mGrounded && jump && HasDoubleJump)
        {
            mRigidBody2D.AddForce(new Vector2(0f, mJumpForce));
            HasDoubleJump = false;
        }
        if (mGrounded && jump)
        {
            mGrounded = false;
            mRigidBody2D.AddForce(new Vector2(0f, mJumpForce));
        }
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

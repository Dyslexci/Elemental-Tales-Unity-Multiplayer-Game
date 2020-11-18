using System;
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

public class CharacterControllerPlayer1 : MonoBehaviour
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
    [SerializeField] private Transform frontCheck;
    [SerializeField] private Rigidbody2D mRigidBody2D;
    private int currentCollectibles1;

    [Header("Movement")]
    [Space]

    private bool mFacingRight = true;
    private bool mGrounded;
    private const float mGroundedRadius = 0.05f;
    private const float mCeilingRadius = 0.2f;
    private Vector3 mVelocity = Vector3.zero;
    private Boolean HasDoubleJump = true;
    private float JumpForce;
    private bool isStomp = false;
    private Boolean isTouchingFront;
    private bool wallSliding;
    [SerializeField] private float wallSlideSpeed;
    private bool wallJumping;
    private float input;

    [Header("HUD")]
    [Space]

    [SerializeField] private int currentHealth;
    [SerializeField] private int currentMana;
    private string currentElement;
    [SerializeField] private int numOfHealthContainers;
    [SerializeField] private int numOfManaContainers;
    [SerializeField] private UnityEngine.UI.Image[] heartContainers;

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
        changeElement("Air");
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
        input = Input.GetAxisRaw("Horizontal");
        bool wasTouchingGround = mGrounded;
        isTouchingFront = false;
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
                    OnLandEvent.Invoke();
            }
        }

        if (!mGrounded && mRigidBody2D.velocity.y < 0)
        {
            mRigidBody2D.gravityScale = (float)4.5;
        }
        else
        {
            mRigidBody2D.gravityScale = 3;
        }

        Collider2D[] frontColliders = Physics2D.OverlapCircleAll(frontCheck.position, mGroundedRadius, mGroundIdentifier);
        for (int i = 0; i < frontColliders.Length; i++)
        {
            if (frontColliders[i].gameObject.CompareTag("Ground"))
            {
                isTouchingFront = true;
            }
        }

        if (isTouchingFront == true && mGrounded == false)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (wallSliding && !wallJumping)
            wallSlide();

        if (Input.GetButtonDown("Jump") && wallSliding)
        {
            wallJumping = true;
        }

        if (wallJumping)
            wallJumpMethod();
    }

    private void OnDrawGizmosSelected()
    {
        if (frontCheck == null)
            return;
        Gizmos.DrawWireSphere(frontCheck.position, mGroundedRadius);
        Gizmos.DrawWireSphere(mGroundCheck.position, mGroundedRadius);
    }

    private void wallJumpMethod()
    {
        mRigidBody2D.gravityScale = 1;
        if (mFacingRight && input < 0)
        {
            mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0f);
            mRigidBody2D.AddRelativeForce(new Vector2(-m_JumpForce, m_JumpForce));
            wallJumping = false;
        }
        else if (mFacingRight && input > 0)
        {
            mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0f);
            mRigidBody2D.AddRelativeForce(new Vector2(-m_JumpForce / 3, m_JumpForce));
            wallJumping = false;
        }
        else if (mFacingRight)
        {
            mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0f);
            mRigidBody2D.AddRelativeForce(new Vector2(-m_JumpForce, m_JumpForce));
            wallJumping = false;
        }
        else if (!mFacingRight && input < 0)
        {
            mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0f);
            mRigidBody2D.AddRelativeForce(new Vector2(m_JumpForce / 3, m_JumpForce));
            wallJumping = false;
        }
        else
        {
            mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0f);
            mRigidBody2D.AddRelativeForce(new Vector2(m_JumpForce, m_JumpForce));
            wallJumping = false;
        }
    }

    private void wallSlide()
    {
        mRigidBody2D.gravityScale = 0;
        mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0f);
        HasDoubleJump = true;
    }

    public void DamageHealth(int damage)
    {
        print("Taken damage");
        health.takeDamage(damage);
    }

    public void stompAttack()
    {
        if (currentElement.Equals("Earth"))
        {
            print("Stomping");
            mRigidBody2D.velocity = new Vector2(0f, 0f);
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
        float moveTemp = move;
        if (isStomp)
            moveTemp = 0;

        Vector3 targetVelocity = new Vector2(moveTemp * 5, mRigidBody2D.velocity.y);
        if (!mGrounded)
        {
            if (!wallSliding)
            {
                if (mRigidBody2D.velocity.y > 0)
                {
                    mRigidBody2D.velocity = Vector3.SmoothDamp(mRigidBody2D.velocity, targetVelocity, ref mVelocity, 0.4f);
                }
                else
                {
                    mRigidBody2D.velocity = Vector3.SmoothDamp(mRigidBody2D.velocity, targetVelocity, ref mVelocity, 0.2f);
                }
                if (move > 0 && !mFacingRight)
                    Flip();
                else if (move < 0 && mFacingRight)
                    Flip();
            }
        }
        else if (!mGrounded && wallJumping)
        {
            if (!wallSliding)
            {
                if (mRigidBody2D.velocity.y > 0)
                {
                    mRigidBody2D.velocity = Vector3.SmoothDamp(mRigidBody2D.velocity, targetVelocity, ref mVelocity, 0.4f);
                }
                else
                {
                    mRigidBody2D.velocity = Vector3.SmoothDamp(mRigidBody2D.velocity, targetVelocity, ref mVelocity, 0.2f);
                }
                if (move > 0 && !mFacingRight)
                    Flip();
                else if (move < 0 && mFacingRight)
                    Flip();
            }
        }
        else
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
            }
            else
            {
                if (mWasCrouching)
                {
                    if (mCrouchColliderToggle != null)
                        mCrouchColliderToggle.enabled = true;
                    mWasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }

            }

            if (!wallSliding)
            {
                mRigidBody2D.velocity = Vector3.SmoothDamp(mRigidBody2D.velocity, targetVelocity, ref mVelocity, mMovementSmoothing);
                if (move > 0 && !mFacingRight)
                    Flip();
                else if (move < 0 && mFacingRight)
                    Flip();
            }
        }



        if (!mGrounded && jump && HasDoubleJump && currentElement.Equals("Air") && !wallSliding && !wallJumping)
        {
            mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0);
            mRigidBody2D.AddRelativeForce(new Vector2(0f, 400));
            HasDoubleJump = false;
        }
        if (mGrounded && jump)
        {
            mGrounded = false;
            mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0);
            mRigidBody2D.AddForce(new Vector2(0f, m_JumpForce));
        }


    }

    public bool checkIsGrounded()
    {
        return mGrounded;
    }

    private void Flip()
    {
        if (wallSliding)
            return;
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

    private void setWallJumpToFalse()
    {
        wallJumping = false;
    }
}

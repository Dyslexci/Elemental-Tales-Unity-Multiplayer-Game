using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 0.1.0
 *    
 *    Controls the players mana and health visual representations on the HUD. Set up for player 1.
 */

[RequireComponent(typeof(ElementController))]
[System.Serializable]
public class Health : MonoBehaviourPun
{
    [SerializeField] private int currentHealth = 3;
    [SerializeField] private int numOfHearts;
    [SerializeField] private int currentMana;
    [SerializeField] private int numOfManas;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private Image[] manas;
    [SerializeField] private Sprite fullMana;
    [SerializeField] private Sprite emptyMana;

    private bool isRespawning = false;

    private void Start()
    {
        hearts = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHearts();
        manas = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getManas();
    }

    private void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        checkHealth();
        checkMana();

        if(currentHealth <= 0 && !isRespawning)
        {
            Debug.Log("Player has died.");
            GameObject.Find("Game Manager").GetComponent<GameMaster>().respawn();
            currentHealth = 2;
            isRespawning = true;
        }
        DrawHealthMana();

    }

    void DrawHealthMana()
    {
        if (!isRespawning)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < currentHealth)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }

                if (i < numOfHearts)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
        }
        for (int i = 0; i < manas.Length; i++)
        {
            if (i < currentMana)
            {
                manas[i].sprite = fullMana;
            }
            else
            {
                manas[i].sprite = emptyMana;
            }

            if (i < numOfManas)
            {
                manas[i].enabled = true;
            }
            else
            {
                manas[i].enabled = false;
            }
        }
    }

    public void resetPlayerAfterDeath()
    {
        isRespawning = false;
    }

    public void takeDamage(int damage)
    {
        currentHealth += damage;
        print(damage * -1 + " damage taken. " + currentHealth + " health remaining.");
    }

    public void takeManaDamage(int damage)
    {
        currentMana += damage;
    }

    public void setHealth(int health)
    {
        currentHealth = health;
    }

    public void setMana(int mana)
    {
        currentMana = mana;
    }

    private void checkHealth()
    {
        if(currentHealth > numOfHearts)
        {
            currentHealth = numOfHearts;
        }
    }

    private void checkMana()
    {
        if (currentMana > numOfManas)
        {
            currentMana = numOfManas;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Health : MonoBehaviour
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

    private void Update()
    {
        checkHealth();
        checkMana();

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            } else
            {
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            } else
            {
                hearts[i].enabled = false;
            }
        }
        for (int i = 0; i < manas.Length; i++)
        {
            if(i < currentMana)
            {
                manas[i].sprite = fullMana;
            } else
            {
                manas[i].sprite = emptyMana;
            }

            if(i < numOfManas)
            {
                manas[i].enabled = true;
            } else
            {
                manas[i].enabled = false;
            }
        }
    }

    public void takeDamage(int damage)
    {
        currentHealth += damage;
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

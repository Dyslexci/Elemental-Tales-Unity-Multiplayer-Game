using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(ElementController))]
public class StatController : MonoBehaviourPun
{
    [Header("Element Variables")]
    public string currentElement;

    [Header("Health Variables")]
    private Image[] hearts;
    public int maxHealth = 3;
    [SerializeField] private int currentHealth;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [Header("Mana Variables")]
    private Image[] manas;
    public int maxMana = 3;
    [SerializeField] private int currentMana;
    [SerializeField] private Sprite fullMana;
    [SerializeField] private Sprite emptyMana;

    PlayerInput inputs;
    GameMaster gameMaster;

    int currentCollectibles1 = 0;
    public bool isRespawning;

    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        hearts = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHearts();
        manas = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getManas();
        gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
        inputs = GetComponent<PlayerInput>();
    }

    void Update()
    {

        // Health script code
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        if (Input.GetKeyUp(KeyCode.J))
        {
            DamageHealth(1);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            DamageMana(1);
        }

        checkStats();

        if (currentHealth <= 0 && !isRespawning)
        {
            Debug.Log("Player has died.");
            gameMaster.respawn();
            inputs.hasControl = false;
            currentHealth = 0;
            isRespawning = true;
        }

        DrawHealthMana();
    }

    public void addCollectible1()
    {
        currentCollectibles1++;
    }

    // Health script functions
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

                if (i < maxHealth)
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

            if (i < maxMana)
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
        currentHealth = 2;
        Debug.Log("Player has respawned, new health is: " + currentHealth);
        inputs.hasControl = true;
    }

    public void DamageHealth(int damage)
    {
        if(currentHealth != 0)
        {
            currentHealth -= damage;
            print(damage * -1 + " damage taken. " + currentHealth + " health remaining.");
        }
    }

    public bool DamageMana(int damage)
    {
        if(currentMana != 0)
        {
            currentMana -= damage;
            return true;
        }
        return false;
    }

    public void setHealth(int health)
    {
        currentHealth = health;
    }

    public void setMana(int mana)
    {
        currentMana = mana;
    }

    public string getElement()
    {
        return currentElement;
    }

    private void checkStats()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }
}

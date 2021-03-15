using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/** 
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.0
 *    
 *    Controls the players mana and health visual representations on the HUD, as well as handling collectibles.
 */

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
    public int currentMana;
    [SerializeField] private Sprite fullMana;
    [SerializeField] private Sprite emptyMana;

    PlayerInput inputs;
    GameMaster gameMaster;

    AudioSource[] injuryAudio;
    AudioSource[] deathAudio;

    int currentCollectibles1 = 0;
    public bool isRespawning;
    public SpriteRenderer playerSprite;

    Color playerColour;

    /// <summary>
    /// Initialises the statistics and HUD objects for this object.
    /// </summary>
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        hearts = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHearts();
        manas = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getManas();
        gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
        inputs = GetComponent<PlayerInput>();
        injuryAudio = gameMaster.injuryAudio;
        deathAudio = gameMaster.deathAudio;

        if (GlobalVariableManager.PlayerColour.Equals("FFFFFF"))
        {
            playerColour = new Color(1, 1, 1);
        }
        if (GlobalVariableManager.PlayerColour.Equals("FFCD81"))
        {
            playerColour = new Color(1, 0.801076f, 0.504717f);
        }
        if (GlobalVariableManager.PlayerColour.Equals("B581FF"))
        {
            playerColour = new Color(0.7103899f, 0.5058824f, 1);
        }
        if (GlobalVariableManager.PlayerColour.Equals("81FAFF"))
        {
            playerColour = new Color(0.5058824f, 0.9781956f, 1);
        }
    }

    /// <summary>
    /// Updates the health/mana, the drawing of the health/mana objects on the HUD, and deals with respawning.
    /// </summary>
    void Update()
    {

        // Health script code
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        checkStats();

        if (currentHealth <= 0 && !isRespawning)
        {
            deathAudio[Random.Range(0, deathAudio.Length)].Play(0);
            Debug.Log("Player has died.");
            gameMaster.respawn();
            inputs.hasControl = false;
            currentHealth = 0;
            isRespawning = true;
        }

        DrawHealthMana();
    }

    /// <summary>
    /// Adds 1 to the collectible1 counter.
    /// </summary>
    public void addCollectible1()
    {
        currentCollectibles1++;
    }

    /// <summary>
    /// Draws the heart and mana objects on the HUD, based on current statistics.
    /// </summary>
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

    /// <summary>
    /// Resets the object to a base post-respawn state.
    /// </summary>
    public void resetPlayerAfterDeath()
    {
        isRespawning = false;
        currentHealth = 2;
        Debug.Log("Player has respawned, new health is: " + currentHealth);
        inputs.hasControl = true;
    }

    /// <summary>
    /// Causes 1 damage to this object.
    /// </summary>
    /// <param name="damage"></param>
    public void DamageHealth(int damage)
    {
        if(currentHealth != 0)
        {
            injuryAudio[Random.Range(0, injuryAudio.Length)].Play(0);
            currentHealth -= damage;
            print(damage * -1 + " damage taken. " + currentHealth + " health remaining.");

            if(damage > 0)
            {
                GetComponent<PlayerInputs>().OnJumpInputDown();
                StartCoroutine(WaitToEndJump(GlobalVariableManager.PlayerColour));
                StartCoroutine(FlashDamagedRed());
            }
        }
    }

    IEnumerator WaitToEndJump(string hex)
    {
        yield return new WaitForSeconds(.2f);
        GetComponent<PlayerInputs>().OnJumpInputUp();
    }

    IEnumerator FlashDamagedRed()
    {
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(.1f);
        playerSprite.color = playerColour;
        yield return new WaitForSeconds(.1f);
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(.1f);
        playerSprite.color = playerColour;
        yield return new WaitForSeconds(.1f);
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(.1f);
        playerSprite.color = playerColour;
    }

    /// <summary>
    /// Causes 1 damage of mana to this object.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool DamageMana(int damage)
    {
        if(currentMana != 0)
        {
            currentMana -= damage;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Sets the health of this object to the parameter.
    /// </summary>
    /// <param name="health"></param>
    public void setHealth(int health)
    {
        currentHealth = health;
    }

    /// <summary>
    /// Sets the mana of this object to the parameter.
    /// </summary>
    /// <param name="mana"></param>
    public void setMana(int mana)
    {
        currentMana = mana;
    }

    public void IncreaseMaxHealth()
    {
        maxHealth += 1;
        currentHealth = maxHealth;
    }

    public void IncreaseMaxMana()
    {
        maxMana += 1;
    }

    /// <summary>
    /// Returns the current element of this object.
    /// </summary>
    /// <returns></returns>
    public string getElement()
    {
        return currentElement;
    }

    /// <summary>
    /// Ensures this objects health and mana can never exceed their maximum values.
    /// </summary>
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

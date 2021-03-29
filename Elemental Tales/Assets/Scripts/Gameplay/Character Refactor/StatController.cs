using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 1.0.2
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

    public float maxHealth = 3;
    private SafeFloat currentHealth;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [Header("Mana Variables")]
    private Image[] manas;

    public float maxMana = 3;
    public SafeFloat currentMana;
    [SerializeField] private Sprite fullMana;
    [SerializeField] private Sprite emptyMana;

    private PlayerInput inputs;
    private GameMaster gameMaster;

    private AudioSource[] injuryAudio;
    private AudioSource[] deathAudio;
    public Animator camAnim;

    public bool isRespawning;
    public SpriteRenderer playerSprite;

    private Color playerColour;

    /// <summary>
    /// Initialises the statistics and HUD objects for this object.
    /// </summary>
    private void Start()
    {
        camAnim = GameObject.Find("Virtual Camera").GetComponentInChildren<Animator>();
        currentHealth = new SafeFloat(maxHealth);
        currentMana = new SafeFloat(maxMana);
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
    private void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            return;

        checkStats();
        CheckForDeath();
        DrawHealthMana();
    }

    private void CheckForDeath()
    {
        if (currentHealth.GetValue() <= 0 && !isRespawning)
        {
            deathAudio[Random.Range(0, deathAudio.Length)].Play(0);
            Debug.Log("Player has died.");
            gameMaster.respawn();
            inputs.hasControl = false;
            currentHealth = new SafeFloat(0);
            isRespawning = true;
            playerSprite.color = playerColour;
        }
    }

    /// <summary>
    /// Refactored by Adnan
    /// Draws the heart and mana objects on the HUD, based on current statistics.
    /// </summary>
    private void DrawHealthMana()
    {
        if (!isRespawning)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].sprite = i < currentHealth.GetValue() ? fullHeart : emptyHeart;
                hearts[i].enabled = i < maxHealth ? true : false;
            }
        }

        for (int i = 0; i < manas.Length; i++)
        {
            manas[i].sprite = i < currentMana.GetValue() ? fullMana : emptyMana;
            manas[i].enabled = i < maxMana ? true : false;
        }
    }

    /// <summary>
    /// Resets the object to a base post-respawn state.
    /// </summary>
    public void resetPlayerAfterDeath()
    {
        isRespawning = false;
        currentHealth = new SafeFloat(2);
        Debug.Log("Player has respawned, new health is: " + currentHealth);
        inputs.hasControl = true;
    }

    /// <summary>
    /// Causes 1 damage to this object.
    /// </summary>
    /// <param name="damage"></param>
    public void DamageHealth(int _damage)
    {
        if (currentHealth.GetValue() != 0)
        {
            SafeFloat damage = new SafeFloat(_damage);
            injuryAudio[Random.Range(0, injuryAudio.Length)].Play(0);
            currentHealth -= damage;
            camAnim.SetTrigger("Trigger");

            if (damage.GetValue() > 0)
            {
                GetComponent<PlayerInputs>().OnJumpInputDown();
                StartCoroutine(WaitToEndJump(GlobalVariableManager.PlayerColour));
                StartCoroutine(FlashDamagedRed());
            }
        }
    }

    private IEnumerator WaitToEndJump(string hex)
    {
        yield return new WaitForSeconds(.2f);
        GetComponent<PlayerInputs>().OnJumpInputUp();
    }

    private IEnumerator FlashDamagedRed()
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
    public bool DamageMana(int _damage)
    {
        SafeFloat damage = new SafeFloat(_damage);
        if (currentMana.GetValue() != 0)
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
        currentHealth = new SafeFloat(health);
    }

    /// <summary>
    /// Sets the mana of this object to the parameter.
    /// </summary>
    /// <param name="mana"></param>
    public void setMana(int mana)
    {
        currentMana = new SafeFloat(mana);
    }

    public void IncreaseMaxHealth()
    {
        maxHealth += 1;
        currentHealth = new SafeFloat(maxHealth);
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
        if (currentHealth.GetValue() > maxHealth)
        {
            currentHealth = new SafeFloat(maxHealth);
        }
        if (currentMana.GetValue() > maxMana)
        {
            currentMana = new SafeFloat(maxMana);
        }
    }
}
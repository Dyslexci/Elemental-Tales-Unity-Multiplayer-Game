using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 *    @author Matthew Ahearn
 *    @since 1.3.0
 *    @version 1.0.2
 *
 *    Allows object this script is attached to to be disabled when the players have enough keys to pass through.
 */

public class CurrencyDoor : MonoBehaviourPun
{
    [SerializeField] private Transform pos;
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private LayerMask layer;

    public int keyCost;

    private TMP_Text hintText;
    private GameObject hintHolder;
    private CanvasGroup panel;
    private Image hintImage;
    private bool isDisplayingHint;

    private bool playerPresent = false;
    private bool playerWasPresent;

    /// <summary>
    /// Initiate values
    /// </summary>
    private void Start()
    {
        hintText = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintText();
        hintHolder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().GetHintHolder();
        hintImage = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintContainer();
        panel = hintHolder.GetComponentInChildren<CanvasGroup>();
    }

    /// <summary>
    /// Checks for player presence and whether to fade in or out the signs
    /// </summary>
    private void FixedUpdate()
    {
        playerWasPresent = playerPresent;
        playerPresent = false;
        checkPresent();
        CheckForPlayerInteraction();
    }

    private void CheckForPlayerInteraction()
    {
        if (playerPresent && Input.GetKeyDown(KeyCode.E) && GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<StatController>().currentMana.GetValue() >= keyCost)
        {
            GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<StatController>().DamageMana(keyCost);
            StopCoroutine(WaitHideHint());
            StopCoroutine(FadeHintHolder());
            StopCoroutine(JumpInHintHolder());
            hintHolder.SetActive(false);
            panel.alpha = 1;
            isDisplayingHint = false;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Refactored by Adnan
    /// Coroutine displaying the hint to the player.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitHideHint()
    {
        hintHolder.SetActive(true);
        StartCoroutine("JumpInHintHolder");

        float currentMana = GameObject.Find("Game Manager").GetComponent<GameMaster>().getPlayer().GetComponent<StatController>().currentMana.GetValue();
        string t = ("<color=#ffffff>Press E to spend <color=#ffeb04>" + keyCost + " Keystones <color=#ffffff>opening the door!");
        string f = ("<color=#ffffff>You need <color=#ffeb04>" + keyCost + " Keystones <color=#ffffff>to open the door!");

        hintText.text = currentMana >= keyCost ? t : f;

        yield return new WaitForSeconds(2);
        StartCoroutine("FadeHintHolder");
    }

    /// <summary>
    /// Coroutine making the hint jump into place.
    /// </summary>
    /// <returns></returns>
    private IEnumerator JumpInHintHolder()
    {
        hintImage.transform.localScale = new Vector3(5, 5, 5);

        while (hintImage.transform.localScale.x > 1)
        {
            yield return new WaitForFixedUpdate();
            hintImage.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        }
        hintImage.transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Coroutine making the hint fade out after it has been on screen enough time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeHintHolder()
    {
        while (panel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            panel.alpha -= 0.05f;
        }

        hintHolder.SetActive(false);
        panel.alpha = 1;
        isDisplayingHint = false;
    }

    /// <summary>
    /// Checks for the players presence based off Physics2D collider circles and displays the hint if the player has entered the switch collider.
    /// </summary>
    private void checkPresent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.position, radius, layer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player")
            {
                if (colliders[i].gameObject.GetPhotonView().IsMine)
                {
                    if (isDisplayingHint == false && playerWasPresent == false)
                    {
                        isDisplayingHint = true;
                        GameObject.Find("Game Manager").GetComponent<GameMaster>().hintSound.Play(0);
                        StartCoroutine(WaitHideHint());
                    }
                    playerPresent = true;
                    return;
                }
            }
        }
    }
}
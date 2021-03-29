using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 *    @author Matthew Ahearn
 *    @since 1.0.0
 *    @version 2.1.0
 *
 *    Provides logic for moving the pushable object while a player object is interacting with it. While the player is interacting with it, it inherits the players movement
 *    vectors, performs its own collision checks and combines the two vectors to move.
 */

[RequireComponent(typeof(CharacterControllerRaycast))]
public class PushableObject : MonoBehaviour
{
    public float gravity = 12;
    private CharacterControllerRaycast controller;
    private Vector3 velocity;
    private string hintTooltip;
    private TMP_Text hintText;
    private GameObject hintHolder;
    private CanvasGroup panel;
    private Image hintImage;
    private bool isDisplayingHint;

    /// <summary>
    /// Initialises the related HUD objects and sets the controller.
    /// </summary>
    private void Start()
    {
        hintText = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintText();
        hintHolder = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().GetHintHolder();
        hintImage = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>().getHintContainer();
        controller = GetComponent<CharacterControllerRaycast>();
        panel = hintHolder.GetComponentInChildren<CanvasGroup>();
    }

    /// <summary>
    /// Performs hint display logic and checks for collisions, as well as providing gravity.
    /// </summary>
    private void FixedUpdate()
    {
        velocity += Vector3.down * gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, false);
        if (controller.collisions.below)
        {
            velocity = Vector3.zero;
        }

        DisplayHint();
    }

    private void DisplayHint()
    {
        if (!controller.collisions.wasDisplayingHint && controller.collisions.shouldDisplayHint && isDisplayingHint == false)
        {
            isDisplayingHint = true;
            GameObject.Find("Game Manager").GetComponent<GameMaster>().hintSound.Play(0);
            StartCoroutine("WaitHideHint");
        }
    }

    /// <summary>
    /// Coroutine creating the hint.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitHideHint()
    {
        hintHolder.SetActive(true);
        StartCoroutine("JumpInHintHolder");
        hintText.text = "<color=#ffffff>Hold SHIFT to <color=#ffeb04> grab and move <color=#ffffff>objects!";
        yield return new WaitForSeconds(2);
        StartCoroutine("FadeHintHolder");
    }

    /// <summary>
    /// Coroutine jumping the hint onto the screen.
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
    /// Coroutine fading the hint out.
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
    /// Inherits the pushers current movement and passes it to the pushable object's controller.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public Vector2 Push(Vector2 amount)
    {
        return controller.Move(amount, false);
    }
}
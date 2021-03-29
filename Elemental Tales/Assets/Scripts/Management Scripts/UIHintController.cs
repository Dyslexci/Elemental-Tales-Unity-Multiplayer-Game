using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 *    @author Matthew Ahearn
 *    @since 2.4.0
 *    @version 1.0.0
 *
 *    Handles displaying the main hint on the top of the UI.
 */

public class UIHintController : MonoBehaviour
{
    private getHUDComponents HUDComponents;

    private TMP_Text hintText;
    private GameObject hintHolder;
    private CanvasGroup panel;
    private Image hintImage;
    public bool isDisplayingHint;

    private void Start()
    {
        HUDComponents = GameObject.Find("PlayerHUDObject").GetComponent<getHUDComponents>();

        hintText = HUDComponents.getHintText();
        hintHolder = HUDComponents.GetHintHolder();
        hintImage = HUDComponents.getHintContainer();
        panel = hintHolder.GetComponentInChildren<CanvasGroup>();
    }

    /// <summary>
    /// Called by other classes, stops running the current hint (if any) and begins displaying the new hint.
    /// </summary>
    /// <param name="textString"></param>
    /// <param name="displayTime"></param>
    public void StartHintDisplay(string textString, int displayTime)
    {
        StopAllCoroutines();
        StartCoroutine(WaitHideHint(textString, displayTime));
    }

    /// <summary>
    /// Coroutine displaying the hint to the player.
    /// </summary>
    /// <param name="textString"></param>
    /// <param name="displayTime"></param>
    /// <returns></returns>
    private IEnumerator WaitHideHint(string textString, int displayTime)
    {
        hintHolder.SetActive(true);
        StartCoroutine("JumpInHintHolder");
        hintText.text = textString;
        yield return new WaitForSeconds(displayTime);
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
}
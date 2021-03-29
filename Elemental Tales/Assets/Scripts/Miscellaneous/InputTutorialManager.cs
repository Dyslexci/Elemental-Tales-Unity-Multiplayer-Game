using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
 *    @author Matthew Ahearn
 *    @since 2.3.0
 *    @version 1.0.2
 *
 *    Checks for player first-time variables, removes items from the checklist on completion, and adds those variables to the playerprefs file to ensure the player is never reminded again.
 */

public class InputTutorialManager : MonoBehaviour
{
    public Image tutorial1Image;
    public Image tutorial2Image;
    public Image tutorial3Image;
    public Image tutorial4Image;
    public CanvasGroup tutorial1Panel;
    public CanvasGroup tutorial2Panel;
    public CanvasGroup tutorial3Panel;
    public CanvasGroup tutorial4Panel;
    private GameMaster gameMaster;

    private void Start()
    {
        gameMaster = GameObject.Find("Game Manager").GetComponent<GameMaster>();
        if (PlayerPrefs.HasKey("hasWalked"))
        {
            tutorial1Panel.gameObject.SetActive(false);
        }
        if (PlayerPrefs.HasKey("hasJumped"))
        {
            tutorial2Panel.gameObject.SetActive(false);
        }
        if (PlayerPrefs.HasKey("hasDoubleJumped"))
        {
            tutorial3Panel.gameObject.SetActive(false);
        }
        if (PlayerPrefs.HasKey("hasWallJumped"))
        {
            tutorial4Panel.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        CheckTutorialStatus();
    }

    private void CheckTutorialStatus()
    {
        if (gameMaster.hasWalked)
        {
            StartCoroutine(FadeOutWalkTut());
        }
        if (gameMaster.hasJumped)
        {
            StartCoroutine(FadeOutJumpTut());
        }
        if (gameMaster.hasDoubledJumped)
        {
            StartCoroutine(FadeOutDoubleJumpTut());
        }
        if (gameMaster.hasWallClimbed)
        {
            StartCoroutine(FadeOutWallClimbTut());
        }
    }

    private IEnumerator FadeOutWalkTut()
    {
        PlayerPrefs.SetInt("hasWalked", 1);
        tutorial1Image.color = new Color(0.4601174f, 1, 0.25f);
        yield return new WaitForSeconds(.8f);

        while (tutorial1Panel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            tutorial1Panel.alpha -= .05f;
        }
    }

    private IEnumerator FadeOutJumpTut()
    {
        PlayerPrefs.SetInt("hasJumped", 1);
        tutorial2Image.color = new Color(0.4601174f, 1, 0.25f);
        yield return new WaitForSeconds(.8f);

        while (tutorial2Panel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            tutorial2Panel.alpha -= .05f;
        }
    }

    private IEnumerator FadeOutDoubleJumpTut()
    {
        PlayerPrefs.SetInt("hasDoubleJumped", 1);
        tutorial3Image.color = new Color(0.4601174f, 1, 0.25f);
        yield return new WaitForSeconds(.8f);

        while (tutorial3Panel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            tutorial3Panel.alpha -= .05f;
        }
    }

    private IEnumerator FadeOutWallClimbTut()
    {
        PlayerPrefs.SetInt("hasWallJumped", 1);
        tutorial4Image.color = new Color(0.4601174f, 1, 0.25f);
        yield return new WaitForSeconds(.8f);

        while (tutorial4Panel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            tutorial4Panel.alpha -= .05f;
        }
    }
}
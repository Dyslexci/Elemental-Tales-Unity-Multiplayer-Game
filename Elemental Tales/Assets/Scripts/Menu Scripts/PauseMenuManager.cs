using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 *    @author Matthew Ahearn
 *    @since 1.1.0
 *    @version 1.0.1
 *
 *    Contains methods for buttons to display various elements in the pause screen.
 */

public class PauseMenuManager : MonoBehaviour
{
    public CanvasGroup skillPopUp;
    public TMP_Text skillPopUpText;
    public TMP_Text skillTitle;
    public Button restartButton;

    private bool popUpOpen;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && popUpOpen)
            ClosePopUp();
    }

    private void Start()
    {
        skillTitle.text = "";
        if (!PhotonNetwork.IsMasterClient)
            restartButton.interactable = false;
    }

    /// <summary>
    /// Refactored by Adnan
    /// Forwards display text as a parameter and displays it
    /// </summary>
    /// <param name="s"></param>
    public void displayPopUp(string s)
    {
        skillPopUpText.text = s;
        StartCoroutine(DisplayPopUp());
    }

    /// <summary>
    /// Refactored by Adnan
    /// Changes title of the button from Hover event
    /// </summary>
    /// <param name="s"></param>
    public void changeTitle(string s)
    {
        skillTitle.text = s;
    }

    public void ClosePopUp()
    {
        StartCoroutine(RemovePopUp());
    }

    private IEnumerator DisplayPopUp()
    {
        popUpOpen = true;
        yield return new WaitForFixedUpdate();
        skillPopUp.gameObject.SetActive(true);
    }

    private IEnumerator RemovePopUp()
    {
        popUpOpen = false;
        yield return new WaitForFixedUpdate();
        skillPopUp.gameObject.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuManager : MonoBehaviour
{
    public CanvasGroup skillPopUp;
    public TMP_Text skillPopUpText;
    public TMP_Text skillTitle;

    bool popUpOpen;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && popUpOpen)
        {
            ClosePopUp();
        }
    }

    private void Start()
    {
        skillTitle.text = "";
    }

    public void DisplayDoubleJumpPopUp()
    {
        skillPopUpText.text = "Press <color=#ffeb04>Space<color=#ffffff> in mid-air to perform a <color=#ffeb04>Double Jump<color=#ffffff>!";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayDashPopUp()
    {
        skillPopUpText.text = "Press <color=#ffeb04>Left Ctrl<color=#ffffff> to <color=#ffeb04>Dash<color=#ffffff> to the side!";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayBashPopUp()
    {
        skillPopUpText.text = "Hold <color=#ffeb04>Right Click<color=#ffffff> near targets to perform a <color=#ffeb04>Bash Leap<color=#ffffff>!";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayStompPopUp()
    {
        skillPopUpText.text = "Press <color=#ffeb04>S<color=#ffffff> in mid-air to perform a <color=#ffeb04>Stomp Attack<color=#ffffff>!";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayAttackPopUp()
    {
        skillPopUpText.text = "Press <color=#ffeb04>Left Click<color=#ffffff> to perform a <color=#ffeb04>Spirit Attack<color=#ffffff>!";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayAirPopUp()
    {
        skillPopUpText.text = "Press <color=#ffeb04>1<color=#ffffff> to active the power of <color=#ffeb04>Air<color=#ffffff>!";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayEarthPopUp()
    {
        skillPopUpText.text = "Press <color=#ffeb04>3<color=#ffffff> to activate the power of <color=#ffeb04>Earth<color=#ffffff>!";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayWaterPopUp()
    {
        skillPopUpText.text = "Press <color=#ffeb04>2<color=#ffffff> to activate the power of <color=#ffeb04>Water<color=#ffffff>!";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayFirePopUp()
    {
        skillPopUpText.text = "Press <color=#ffeb04>4<color=#ffffff> to activate the power of <color=#ffeb04>Fire<color=#ffffff>!";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayTimerPopUp()
    {
        skillPopUpText.text = "Shows the <color=#ffeb04>Current Playtime<color=#ffffff> of this <color=#ffeb04>Level<color=#ffffff>";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayPercentagePopUp()
    {
        skillPopUpText.text = "Indicates how much of the <color=#ffeb04>Level<color=#ffffff> you have discovered";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayDeathsPopUp()
    {
        skillPopUpText.text = "Shows how many times you have <color=#ffeb04>Respawned<color=#ffffff>";
        StartCoroutine(DisplayPopUp());
    }

    public void DisplayCollectible1PopUp()
    {
        skillPopUpText.text = "<color=#ffeb04>Elemental Shards<color=#ffffff> track your <color=#ffeb04>Score<color=#ffffff>";
        StartCoroutine(DisplayPopUp());
    }

    public void ChangeDoubleJumpTitle()
    {
        skillTitle.text = "Double Jump";
    }

    public void ChangeDashTitle()
    {
        skillTitle.text = "Dash";
    }

    public void ChangeBashTitle()
    {
        skillTitle.text = "Bash";
    }

    public void ChangeStompTitle()
    {
        skillTitle.text = "Stomp";
    }

    public void ChangeAttackTitle()
    {
        skillTitle.text = "Spirit Attack";
    }

    public void ChangeAirTitle()
    {
        skillTitle.text = "Air";
    }

    public void ChangeEarthTitle()
    {
        skillTitle.text = "Earth";
    }

    public void ChangeWaterTitle()
    {
        skillTitle.text = "Water";
    }

    public void ChangeFireTitle()
    {
        skillTitle.text = "Fire";
    }

    public void ClosePopUp()
    {
        StartCoroutine(RemovePopUp());
    }

    IEnumerator DisplayPopUp()
    {
        popUpOpen = true;
        yield return new WaitForFixedUpdate();
        skillPopUp.gameObject.SetActive(true);
    }

    IEnumerator RemovePopUp()
    {
        popUpOpen = false;
        yield return new WaitForFixedUpdate();
        skillPopUp.gameObject.SetActive(false);
    }
}

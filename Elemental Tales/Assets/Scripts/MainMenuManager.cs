using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startGameScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject exitScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private PanelFader fader;

    private void Start()
    {
        startGameScreen.SetActive(false);
        optionsScreen.SetActive(false);
        exitScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void quitGame()
    {
        menuScreen.SetActive(false);
        exitScreen.SetActive(true);
    }

    public void quitCancel()
    {
        menuScreen.SetActive(true);
        exitScreen.SetActive(false);
    }

    public void quitConfirm()
    {
        Application.Quit();
    }

    public void startGame()
    {
        //Load up lobby manager
    }

    public void optionsMenu()
    {
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void optionsCancel()
    {
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }
}

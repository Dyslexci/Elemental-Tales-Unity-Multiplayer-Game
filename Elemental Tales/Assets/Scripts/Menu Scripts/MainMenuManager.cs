using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string version;
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private GameObject startGameScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject exitScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private PanelFader faderStart;
    [SerializeField] private PanelFader faderOptions;
    [SerializeField] private PanelFader faderExit;
    [SerializeField] private PanelFader faderMenu;
    [SerializeField] private GameObject menuCanvas;

    private void Start()
    {
        startGameScreen.SetActive(false);
        optionsScreen.SetActive(false);
        exitScreen.SetActive(false);
        menuScreen.SetActive(true);
        versionText.text = "v"+version;
    }

    public void quitGame()
    {
        exitScreen.SetActive(true);
        menuScreen.SetActive(false);
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
        menuScreen.SetActive(false);
        startGameScreen.SetActive(true);
    }

    public void quitStartGame()
    {
        startGameScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void optionsMenu()
    {
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }
}

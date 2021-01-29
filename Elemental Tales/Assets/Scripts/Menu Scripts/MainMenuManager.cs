using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Versions")]
    [SerializeField] private string version;
    [SerializeField] private TMP_Text versionText;

    [Header("Main Screen")]
    [SerializeField] private GameObject menuScreen;
    public GameObject menuBlurPanel;
    public CanvasGroup menuFaderPanel;

    [Header("Start Game Screen")]
    [SerializeField] private GameObject startGameScreen;
    public GameObject startBlurPanel;
    public CanvasGroup startFaderPanel;

    [Header("Options Screen")]
    [SerializeField] private GameObject optionsScreen;
    public GameObject optionsBlurPanel;
    public CanvasGroup optionsFaderPanel;

    [Header("Exit Screen")]
    [SerializeField] private GameObject exitScreen;
    public GameObject exitBlurPanel;
    public CanvasGroup exitFaderPanel;



    [SerializeField] private GameObject notesMenu;
    public GameObject notesBlurPanel;
    public CanvasGroup NotesFaderPanel;
    

    private void Start()
    {
        startGameScreen.SetActive(false);
        optionsScreen.SetActive(false);
        exitScreen.SetActive(false);
        menuScreen.SetActive(true);
        notesMenu.SetActive(false);

        versionText.text = "v"+version;
    }

    public void quitGame()
    {
        exitScreen.SetActive(true);
        menuScreen.SetActive(false);
        StartCoroutine(FadeInPanel(exitFaderPanel, exitBlurPanel));
    }

    public void quitCancel()
    {
        StartCoroutine(FadeOutPanel(exitFaderPanel, exitBlurPanel, exitScreen, menuScreen));
    }

    public void quitConfirm()
    {
        Application.Quit();
    }

    public void startGame()
    {
        menuScreen.SetActive(false);
        startGameScreen.SetActive(true);
        StartCoroutine(FadeInPanel(startFaderPanel, startBlurPanel));
    }

    public void quitStartGame()
    {
        StartCoroutine(FadeOutPanel(startFaderPanel, startBlurPanel, startGameScreen, menuScreen));
    }

    public void openNotes()
    {
        menuScreen.SetActive(false);
        notesMenu.SetActive(true);
        StartCoroutine(FadeInPanel(NotesFaderPanel, notesBlurPanel));
    }

    public void closeNotes()
    {
        StartCoroutine(FadeOutPanel(NotesFaderPanel, notesBlurPanel, notesMenu, menuScreen));
    }

    public void optionsMenu()
    {
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
        StartCoroutine(FadeInPanel(optionsFaderPanel, optionsBlurPanel));
    }

    public void CloseOptionsMenu()
    {
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
        StartCoroutine(FadeOutPanel(optionsFaderPanel, optionsBlurPanel, optionsScreen, menuScreen));
    }

    IEnumerator FadeInPanel(CanvasGroup panel, GameObject panelBlur)
    {
        panelBlur.SetActive(false);
        panel.gameObject.SetActive(true);
        panel.alpha = 0;
        while (panel.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            panel.alpha += 0.2f;
        }
        panel.alpha = 1;
        panel.gameObject.SetActive(false);
        panelBlur.SetActive(true);
    }

    IEnumerator FadeOutPanel(CanvasGroup panel, GameObject panelBlur, GameObject fromPanel, GameObject toPanel)
    {
        panelBlur.SetActive(false);
        panel.gameObject.SetActive(true);
        panel.alpha = 1;
        while (panel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            panel.alpha -= 0.2f;
        }
        panel.alpha = 0;
        fromPanel.SetActive(false);
        toPanel.SetActive(true);
    }
}

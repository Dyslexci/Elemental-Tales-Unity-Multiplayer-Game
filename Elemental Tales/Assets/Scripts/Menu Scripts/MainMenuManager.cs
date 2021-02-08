using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 1.2.0
 *    
 *    Manages the main menu - opening and closing panels and dealing with the actions of each button.
 */
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
    
    /// <summary>
    /// Sets the main menu to its default starting state.
    /// </summary>
    private void Start()
    {
        startGameScreen.SetActive(false);
        optionsScreen.SetActive(false);
        exitScreen.SetActive(false);
        menuScreen.SetActive(true);
        notesMenu.SetActive(false);

        versionText.text = "v"+version;
    }

    /// <summary>
    /// Opens tthe exit screen questioning if the player wants to quit the game.
    /// </summary>
    public void quitGame()
    {
        exitScreen.SetActive(true);
        menuScreen.SetActive(false);
        StartCoroutine(FadeInPanel(exitFaderPanel, exitBlurPanel));
    }

    /// <summary>
    /// Cancels quitting the game.
    /// </summary>
    public void quitCancel()
    {
        StartCoroutine(FadeOutPanel(exitFaderPanel, exitBlurPanel, exitScreen, menuScreen));
    }

    /// <summary>
    /// Confirms quitting the game.
    /// </summary>
    public void quitConfirm()
    {
        Application.Quit();
    }

    /// <summary>
    /// Opens the start game panel.
    /// </summary>
    public void startGame()
    {
        menuScreen.SetActive(false);
        startGameScreen.SetActive(true);
        StartCoroutine(FadeInPanel(startFaderPanel, startBlurPanel));
    }

    /// <summary>
    /// Exits the start game panel.
    /// </summary>
    public void quitStartGame()
    {
        StartCoroutine(FadeOutPanel(startFaderPanel, startBlurPanel, startGameScreen, menuScreen));
    }

    /// <summary>
    /// Opens the notes panel.
    /// </summary>
    public void openNotes()
    {
        menuScreen.SetActive(false);
        notesMenu.SetActive(true);
        StartCoroutine(FadeInPanel(NotesFaderPanel, notesBlurPanel));
    }

    /// <summary>
    /// Closes the notes panel.
    /// </summary>
    public void closeNotes()
    {
        StartCoroutine(FadeOutPanel(NotesFaderPanel, notesBlurPanel, notesMenu, menuScreen));
    }

    /// <summary>
    /// Opens the options menu.
    /// </summary>
    public void optionsMenu()
    {
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
        StartCoroutine(FadeInPanel(optionsFaderPanel, optionsBlurPanel));
    }

    /// <summary>
    /// Closes the options menu.
    /// </summary>
    public void CloseOptionsMenu()
    {
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
        StartCoroutine(FadeOutPanel(optionsFaderPanel, optionsBlurPanel, optionsScreen, menuScreen));
    }

    /// <summary>
    /// Fades in a panel and activates its blur panel when finished fading.
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="panelBlur"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Fades out a panel, disabling its blur panel at the start.
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="panelBlur"></param>
    /// <param name="fromPanel"></param>
    /// <param name="toPanel"></param>
    /// <returns></returns>
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

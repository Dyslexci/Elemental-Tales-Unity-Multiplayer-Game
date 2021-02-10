using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/** 
 *    @author Matthew Ahearn
 *    @since 0.0.0
 *    @version 2.0.0
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

    [Header("Notes Screen")]
    [SerializeField] private GameObject notesMenu;
    public GameObject notesBlurPanel;
    public CanvasGroup NotesFaderPanel;

    [Header("Misc")]
    [SerializeField] Camera mainCam;
    public CanvasGroup startBlackPanel;
    public TMP_Text startText;
    public CanvasGroup startTextPanel;
    public CanvasGroup mainButtonsPanel;
    public AudioSource buttonClick;
    public AudioSource music;
    float musicStartVolume;

    string activeMenu = "";
    
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
        musicStartVolume = music.volume;
        startBlackPanel.alpha = 1;

        if(!GlobalVariableManager.HasLoaded)
        {
            startBlackPanel.gameObject.SetActive(true);
            StartCoroutine(FadeStartText());
            GlobalVariableManager.HasLoaded = true;
        } else
        {
            startBlackPanel.gameObject.SetActive(true);
            StartCoroutine(FadeIntoMenuAlt());
        }

        mainCam.transform.position = new Vector3(mainCam.transform.position.x, 20.8f, mainCam.transform.position.z);

        versionText.text = "v"+version;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(activeMenu.Equals("mainMenu"))
            {
                buttonClick.Play();
                quitGame();
            } else if(activeMenu.Equals("quitMenu"))
            {
                buttonClick.Play();
                quitCancel();
            } else if(activeMenu.Equals("startMenu"))
            {
                buttonClick.Play();
                quitStartGame();
            } else if(activeMenu.Equals("notesMenu"))
            {
                buttonClick.Play();
                closeNotes();
            } else if(activeMenu.Equals("optionsMenu"))
            {
                buttonClick.Play();
                CloseOptionsMenu();
            }
        }
    }

    /// <summary>
    /// Opens tthe exit screen questioning if the player wants to quit the game.
    /// </summary>
    public void quitGame()
    {
        activeMenu = "quitMenu";
        exitScreen.SetActive(true);
        menuScreen.SetActive(false);
        StartCoroutine(FadeInPanel(exitFaderPanel, exitBlurPanel));
    }

    /// <summary>
    /// Cancels quitting the game.
    /// </summary>
    public void quitCancel()
    {
        activeMenu = "mainMenu";
        StartCoroutine(FadeOutPanel(exitFaderPanel, exitBlurPanel, exitScreen, menuScreen));
    }

    /// <summary>
    /// Confirms quitting the game.
    /// </summary>
    public void quitConfirm()
    {
        StartCoroutine(FadeToApplicationClose());
    }

    /// <summary>
    /// Opens the start game panel.
    /// </summary>
    public void startGame()
    {
        activeMenu = "startMenu";
        menuScreen.SetActive(false);
        startGameScreen.SetActive(true);
        StartCoroutine(FadeInPanel(startFaderPanel, startBlurPanel));
    }

    /// <summary>
    /// Exits the start game panel.
    /// </summary>
    public void quitStartGame()
    {
        activeMenu = "mainMenu";
        StartCoroutine(FadeOutPanel(startFaderPanel, startBlurPanel, startGameScreen, menuScreen));
    }

    /// <summary>
    /// Opens the notes panel.
    /// </summary>
    public void openNotes()
    {
        activeMenu = "notesMenu";
        menuScreen.SetActive(false);
        notesMenu.SetActive(true);
        StartCoroutine(FadeInPanel(NotesFaderPanel, notesBlurPanel));
    }

    /// <summary>
    /// Closes the notes panel.
    /// </summary>
    public void closeNotes()
    {
        activeMenu = "mainMenu";
        StartCoroutine(FadeOutPanel(NotesFaderPanel, notesBlurPanel, notesMenu, menuScreen));
    }

    /// <summary>
    /// Opens the options menu.
    /// </summary>
    public void optionsMenu()
    {
        activeMenu = "optionsMenu";
        this.gameObject.GetComponent<OptionsManager>().ResetOptionsMenu();
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
        StartCoroutine(FadeInPanel(optionsFaderPanel, optionsBlurPanel));
    }

    /// <summary>
    /// Closes the options menu.
    /// </summary>
    public void CloseOptionsMenu()
    {
        activeMenu = "mainMenu";
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

    /// <summary>
    /// Starts off the first launch of the game by fading in and out text and panels, to create a smooth intro splash screen.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeStartText()
    {
        startTextPanel.gameObject.SetActive(true);
        startBlackPanel.gameObject.SetActive(true);
        startTextPanel.alpha = 0f;
        startBlackPanel.alpha = 1f;
        mainButtonsPanel.alpha = 0f;
        yield return new WaitForSeconds(.5f);
        while (startTextPanel.alpha < 1.0f)
        {
            yield return new WaitForFixedUpdate();
            startTextPanel.alpha += 0.01f;
        }
        yield return new WaitForSeconds(1.5f);
        while(startBlackPanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            startBlackPanel.alpha -= 0.01f;
            startTextPanel.alpha -= 0.05f;
        }
        startBlackPanel.gameObject.SetActive(false);
        startTextPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        while(mainButtonsPanel.alpha < 1f)
        {
            yield return new WaitForFixedUpdate();
            mainButtonsPanel.alpha += 0.03f;
        }
        activeMenu = "mainMenu";
    }

    /// <summary>
    /// When the player has already been through the intro splash screen, this simply fades the main menu in from black.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIntoMenuAlt()
    {
        while(startBlackPanel.alpha > 0)
        {
            yield return new WaitForFixedUpdate();
            startBlackPanel.alpha -= 0.01f;
        }
        startBlackPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fades to black and then closes the application.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeToApplicationClose()
    {
        startBlackPanel.alpha = 0;
        startBlackPanel.gameObject.SetActive(true);
        while(startBlackPanel.alpha < 1)
        {
            yield return new WaitForFixedUpdate();
            startBlackPanel.alpha += 0.01f;
            music.volume -= (0.01f * musicStartVolume);
        }
        Application.Quit();
    }
}

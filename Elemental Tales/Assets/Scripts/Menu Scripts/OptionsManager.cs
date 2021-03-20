using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/** 
 *    @author Matthew Ahearn
 *    @since 0.4.0
 *    @version 1.2.2
 *    
 *    Manages the options menu, setting the various option states.
 */
public class OptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private TMP_Text fullscreenToggleText;
    [SerializeField] private TMP_Dropdown resDropdown;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject settingsTab;
    [SerializeField] private GameObject controlsTab;
    [SerializeField] Button settingsButton;
    [SerializeField] Button controlsButton;
    public Slider musicSlider;
    public Slider soundSlider;

    public CanvasGroup pauseMenuPanel;
    public GameObject optionsMenuPanel;

    private List<Resolution> resolutionsTemp;
    private Resolution[] resolutions;
    private Resolution selectedResolution;
    private float currentMusicVolume;
    private float currentSoundVolume;
    private bool vsync = false;
    private bool fullscreenEnabled = true;

    float startingMusicVolume;
    float startingSoundVolume;
    bool startingVsync;
    bool startingFullscreen;

    /// <summary>
    /// Sets the default options.
    /// </summary>
    private void Start()
    {
        if(!PlayerPrefs.HasKey("hasLoadedOnce"))
        {
            PlayerPrefs.SetFloat("currentMusicVolume", 1);
            PlayerPrefs.SetFloat("currentSoundVolume", 1);
            PlayerPrefs.SetInt("fullscreenEnabled", 1);
            PlayerPrefs.SetInt("vSyncEnabled", 0);
            PlayerPrefs.SetInt("hasLoadedOnce", 1);
            Debug.LogError("Initialising first time preferences");
        }

        setMusicVolume(PlayerPrefs.GetFloat("currentMusicVolume"));
        musicSlider.value = PlayerPrefs.GetFloat("currentMusicVolume");
        setSoundVolume(PlayerPrefs.GetFloat("currentSoundVolume"));
        soundSlider.value = PlayerPrefs.GetFloat("currentSoundVolume");
        if (PlayerPrefs.GetInt("fullscreenEnabled") == 0)
        {
            toggleFullscreen();
        }
        if(PlayerPrefs.GetInt("vSyncEnabled") == 0)
        {
            toggleVSync();
        }

        Debug.Log("Current music volume from prefs: " + PlayerPrefs.GetFloat("currentMusicVolume"));
        Debug.Log("Current sound volume from prefs: " + PlayerPrefs.GetFloat("currentSoundVolume"));
        Debug.Log("Fullscreen enabled from prefs: " + PlayerPrefs.GetInt("fullscreenEnabled"));
        Debug.Log("vSync enabled from prefs: " + PlayerPrefs.GetInt("vSyncEnabled"));
        Debug.Log("Has loaded initial prefs: " + PlayerPrefs.GetInt("hasLoadedOnce"));

        Screen.fullScreen = true;
        fullscreenToggleText.text = "On";
        resolutionsTemp = new List<Resolution>();

        //resolutions = Screen.resolutions;
        foreach(Resolution res in Screen.resolutions)
        {
            if (res.refreshRate >= 59.0f && res.refreshRate <= 60.0f)
                resolutionsTemp.Add(res);
            if (res.refreshRate >= 143.0f && res.refreshRate <= 144.0f)
                resolutionsTemp.Add(res);
        }
        resolutions = resolutionsTemp.ToArray();

        resDropdown.ClearOptions();
        List<string> resOptions = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = resolutions.Length - 1; i > 0; i--)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " [" + resolutions[i].refreshRate + "Hz]";
            resOptions.Add(option);
            if (resolutions[i].width == 1080 && resolutions[i].height == 1920)
            {
                currentResolutionIndex = i;
            }
        }
        resDropdown.AddOptions(resOptions);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();

        openSettingsTab();
    }

    public void ResetOptionsMenu()
    {
        startingFullscreen = fullscreenEnabled;
        startingVsync = vsync;
        startingMusicVolume = currentMusicVolume;
        startingSoundVolume = currentSoundVolume;
        openSettingsTab();
    }

    /// <summary>
    /// Sets the application resolution.
    /// </summary>
    public void setResolution()
    {
        int resolutionIndex = resolutions.Length - resDropdown.value - 1;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Resolution: " + resolution.width + "x" + resolution.height);
    }

    /// <summary>
    /// Toggles the application fullscreen mode.
    /// </summary>
    public void toggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if (Screen.fullScreen)
        {
            fullscreenToggleText.text = "Off";
            fullscreenEnabled = false;
        }
        else
        {
            fullscreenToggleText.text = "On";
            fullscreenEnabled = true;
        }
    }

    /// <summary>
    /// Refactored by Adnan
    /// Toggles the application VSync.
    /// </summary>
    public void toggleVSync()
    {
        vsync = !vsync;
    }

    /// <summary>
    /// Sets the local music volume.
    /// </summary>
    /// <param name="volume"></param>
    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVol", Mathf.Log(volume) * 20);
        currentMusicVolume = volume;
    }

    /// <summary>
    /// Sets the local sounds volume.
    /// </summary>
    /// <param name="volume"></param>
    public void setSoundVolume(float volume)
    {
        audioMixer.SetFloat("soundVol", Mathf.Log(volume) * 20);
        currentSoundVolume = volume;
    }

    /// <summary>
    /// Applies the options.
    /// </summary>
    public void optionsApply()
    {
        openSettingsTab();
        PlayerPrefs.SetFloat("currentMusicVolume", currentMusicVolume);
        PlayerPrefs.SetFloat("currentSoundVolume", currentSoundVolume);
        PlayerPrefs.SetInt("fullscreenEnabled", fullscreenEnabled ? 1 : 0);
        PlayerPrefs.SetInt("vSyncEnabled", vsync ? 1 : 0);
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    /// <summary>
    /// Cancels the options.
    /// </summary>
    public void optionsCancel()
    {
        setMusicVolume(startingMusicVolume);
        musicSlider.value = startingMusicVolume;
        setSoundVolume(startingSoundVolume);
        soundSlider.value = startingSoundVolume;
        if (fullscreenEnabled != startingFullscreen)
            toggleFullscreen();
        if (vsync != startingVsync)
            toggleVSync();
        openSettingsTab();
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void OptionsApplyInGame()
    {
        openSettingsTab();
        PlayerPrefs.SetFloat("currentMusicVolume", currentMusicVolume);
        PlayerPrefs.SetFloat("currentSoundVolume", currentSoundVolume);
        PlayerPrefs.SetInt("fullscreenEnabled", fullscreenEnabled ? 1 : 0);
        PlayerPrefs.SetInt("vSyncEnabled", vsync ? 1 : 0);
        optionsMenuPanel.gameObject.SetActive(false);
        pauseMenuPanel.gameObject.SetActive(true);
    }

    public void OptionsCancelInGame()
    {
        setMusicVolume(startingMusicVolume);
        musicSlider.value = startingMusicVolume;
        setSoundVolume(startingSoundVolume);
        soundSlider.value = startingSoundVolume;
        if (fullscreenEnabled != startingFullscreen)
            toggleFullscreen();
        if (vsync != startingVsync)
            toggleVSync();
        openSettingsTab();
        optionsMenuPanel.gameObject.SetActive(false);
        pauseMenuPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// Opens the settings tab, closes the controls tab.
    /// </summary>
    public void openSettingsTab()
    {
        try
        {
            settingsTab.SetActive(true);
        } catch
        {
            Debug.LogWarning("Warning: OptionsManager has failed to initialise the settingsTab variable. This message should only play during debugging and testing, and is safe to ignore." +
                " If this message shows when the project is due in, please contact Matt.");
        }
        try
        {
            controlsTab.SetActive(false);
        } catch
        {
            Debug.LogWarning("Warning: OptionsManager has failed to initialise the controlsTab variable. This message should only play during debugging and testing, and is safe to ignore." +
                " If this message shows when the project is due in, please contact Matt.");
        }
    }

    /// <summary>
    /// Opens the controls tab, closes the settings tab.
    /// </summary>
    public void openControlsTab()
    {
        settingsTab.SetActive(false);
        controlsTab.SetActive(true);
    }
}

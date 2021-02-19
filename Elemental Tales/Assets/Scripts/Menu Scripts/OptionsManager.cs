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
 *    @version 1.2.1
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

    private const string resolutionWidthPlayerPrefKey = "ResolutionWidth";
    private const string resolutionHeightPlayerPrefKey = "ResolutionHeight";
    private const string resolutionRefreshRatePlayerPrefKey = "RefreshRate";
    private const string fullScreenPlayerPrefKey = "FullScreen";

    private List<Resolution> resolutionsTemp;
    private Resolution[] resolutions;
    private Resolution selectedResolution;
    private float currentVolume;
    private bool vsync = false;

    /// <summary>
    /// Sets the default options.
    /// </summary>
    private void Start()
    {
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
        }
        else
        {
            fullscreenToggleText.text = "On";
        }
    }

    /// <summary>
    /// Toggles the application VSync.
    /// </summary>
    public void toggleVSync()
    {
        if(vsync)
        {

        }
    }

    /// <summary>
    /// Sets the local music volume.
    /// </summary>
    /// <param name="volume"></param>
    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVol", Mathf.Log(volume) * 20);
        currentVolume = volume;
    }

    /// <summary>
    /// Sets the local sounds volume.
    /// </summary>
    /// <param name="volume"></param>
    public void setSoundVolume(float volume)
    {
        audioMixer.SetFloat("soundVol", Mathf.Log(volume) * 20);
    }

    /// <summary>
    /// Applies the options.
    /// </summary>
    public void optionsApply()
    {
        openSettingsTab();
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    /// <summary>
    /// Cancels the options.
    /// </summary>
    public void optionsCancel()
    {
        openSettingsTab();
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
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

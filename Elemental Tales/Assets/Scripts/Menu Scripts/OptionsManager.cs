using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private TMP_Text fullscreenToggleText;
    [SerializeField] private TMP_Dropdown resDropdown;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject settingsTab;
    [SerializeField] private GameObject controlsTab;

    private const string resolutionWidthPlayerPrefKey = "ResolutionWidth";
    private const string resolutionHeightPlayerPrefKey = "ResolutionHeight";
    private const string resolutionRefreshRatePlayerPrefKey = "RefreshRate";
    private const string fullScreenPlayerPrefKey = "FullScreen";

    private List<Resolution> resolutionsTemp;
    private Resolution[] resolutions;
    private Resolution selectedResolution;
    private float currentVolume;
    private bool vsync = false;


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

    public void setResolution()
    {
        int resolutionIndex = resolutions.Length - resDropdown.value - 1;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Resolution: " + resolution.width + "x" + resolution.height);
    }

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

    public void toggleVSync()
    {
        if(vsync)
        {

        }
    }

    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVol", Mathf.Log(volume) * 20);
        currentVolume = volume;
    }

    public void setSoundVolume(float volume)
    {
        audioMixer.SetFloat("soundVol", Mathf.Log(volume) * 20);
    }

    public void optionsApply()
    {
        openSettingsTab();
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void optionsCancel()
    {
        openSettingsTab();
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void openSettingsTab()
    {
        settingsTab.SetActive(true);
        controlsTab.SetActive(false);
    }

    public void openControlsTab()
    {
        settingsTab.SetActive(false);
        controlsTab.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private TMP_Text fullscreenToggleText;
    [SerializeField] private TMP_Dropdown resDropdown;
    [SerializeField] private AudioMixer audioMixer;

    private bool fullscreenOn = true;
    private Resolution[] resolutions;
    private float currentVolume;
    private bool vsync = false;

    private void Start()
    {
        Screen.fullScreen = true;
        fullscreenToggleText.text = "On";

        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> resOptions = new List<string>();

        int currentResolutionIndex = 0;
        for(int i=resolutions.Length-1; i >0;i--)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " [" + resolutions[i].refreshRate + "Hz]";
            resOptions.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resDropdown.AddOptions(resOptions);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();
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
        if(Screen.fullScreen)
        {
            fullscreenToggleText.text = "Off";
        } else
        {
            fullscreenToggleText.text = "On";
        }
    }

    public void toggleVSync()
    {

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
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void optionsCancel()
    {
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsOptions : MonoBehaviour
{
    public Dropdown windowDropDown;
    public Dropdown resolutionDropDown;

    public Resolution[] supportedResolutions;

    Resolution currentResolution;
    FullScreenMode currentFullScreenMode;

    void Awake()
    {
        currentResolution = Screen.currentResolution;
        currentFullScreenMode = Screen.fullScreenMode;

        if (!windowDropDown)
        {
            windowDropDown = GameObject.Find("WindowDropdown").GetComponent<Dropdown>();
        }
        if (!resolutionDropDown)
        {
            resolutionDropDown = GameObject.Find("ResolutionDropdown").GetComponent<Dropdown>();
        }
    }

    void Start()
    {
        InitializeWindowDropdown();
        windowDropDown.onValueChanged.AddListener(delegate {
            WindowValueChangedHandler(windowDropDown);
        });

        supportedResolutions = Screen.resolutions;
        PopulateResolutionDropdown(supportedResolutions);
        resolutionDropDown.onValueChanged.AddListener(delegate {
            ResolutionValueChangedHandler(resolutionDropDown);
        });
    }

    private void InitializeWindowDropdown()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                windowDropDown.value = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                windowDropDown.value = 1;
                break;
            case FullScreenMode.MaximizedWindow:
                windowDropDown.value = 2;
                break;
            case FullScreenMode.Windowed:
                windowDropDown.value = 2;
                break;
            default:    // If we're in unity editor, show this in dropdown
                List<Dropdown.OptionData> editorLockOption = new List<Dropdown.OptionData>();
                Dropdown.OptionData option = new Dropdown.OptionData
                {
                    text = "Unity Editor"
                };
                editorLockOption.Add(option);
                windowDropDown.options = editorLockOption;
                windowDropDown.value = 0;
                break;
        }
    }

    private void PopulateResolutionDropdown(Resolution[] resolutions)
    {
        resolutionDropDown.ClearOptions();
        List<Dropdown.OptionData> resolutionOptions = new List<Dropdown.OptionData>();
        int currentResIndex = 0;

        // Get all supported resolutions
        for (int i = resolutions.Length - 1; i > 0; i--)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = resolutions[i].width + " x " + resolutions[i].height;

            resolutionOptions.Add(option);

            // Find index of our current resolution
            if (resolutions[i].width == currentResolution.width && resolutions[i].height == currentResolution.height)
            {
                currentResIndex = resolutions.Length - i - 1;
            }
        }

        // populate the dropdown
        resolutionDropDown.options = resolutionOptions;

        // Set our current resolution as the selected option
        resolutionDropDown.value = currentResIndex;
    }

    void Destroy()
    {
        windowDropDown.onValueChanged.RemoveAllListeners();
        resolutionDropDown.onValueChanged.RemoveAllListeners();
    }

    private void WindowValueChangedHandler(Dropdown target)
    {
        switch (target.value)
        {
            case 0: // fullscreen
                currentFullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // borderless
                currentFullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // windowed
                currentFullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                break;
        }
        UpdateWindowAndResolution();
    }

    private void ResolutionValueChangedHandler(Dropdown target)
    {
        Resolution newResolution = supportedResolutions[target.options.Count - target.value];

        if (newResolution.width != currentResolution.width || newResolution.height != currentResolution.height)
        {
            currentResolution = newResolution;
            UpdateWindowAndResolution();
        }
    }

    private void UpdateWindowAndResolution()
    {
        Screen.SetResolution(currentResolution.width, currentResolution.height, currentFullScreenMode, currentResolution.refreshRate);

        PlayerPrefs.SetInt("graphics_windowMode", (int)currentFullScreenMode);
        PlayerPrefs.SetString("graphics_resolution", currentResolution.width + "x" + currentResolution.height);
        PlayerPrefs.SetInt("graphics_refreshHz", currentResolution.refreshRate);
    }

    private void LoadGraphicsPrefs()
    {
        int windowMode = PlayerPrefs.GetInt("graphics_windowMode", (int)currentFullScreenMode);
        String resolution = PlayerPrefs.GetString("graphics_resolution", currentResolution.width + "x" + currentResolution.height);
        int refreshRate = PlayerPrefs.GetInt("graphics_refreshHz", 0);
    }
}

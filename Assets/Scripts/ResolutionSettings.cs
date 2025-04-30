using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ResolutionSettings : MonoBehaviour
{
    // Reference to the resolution dropdown UI element
    public TMP_Dropdown resolutionDropdown;

    // Predefined list of supported resolutions
    private List<Resolution> predefinedResolutions = new List<Resolution>()
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1152, height = 648 },
        new Resolution { width = 1024, height = 576 },
        new Resolution { width = 960, height = 540 },
        new Resolution { width = 854, height = 480 },
        new Resolution { width = 640, height = 360 },
        new Resolution { width = 1920, height = 1200 },
        new Resolution { width = 1680, height = 1050 },
        new Resolution { width = 1440, height = 900 },
        new Resolution { width = 1280, height = 1024 },
        new Resolution { width = 1024, height = 768 },
        new Resolution { width = 2560, height = 1080 },
        new Resolution { width = 3440, height = 1440 }
    };

    void Start()
    {
        // Clear existing dropdown options
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();

        // Populate dropdown with all predefined resolutions
        for (int i = 0; i < predefinedResolutions.Count; i++)
        {
            string option = predefinedResolutions[i].width + " x " + predefinedResolutions[i].height;
            options.Add(option);

            // Check if this resolution matches the current screen resolution
            if (predefinedResolutions[i].width == Screen.currentResolution.width &&
                predefinedResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Add options to dropdown and set default selection
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Add listener for resolution changes
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    // Applies the selected resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = predefinedResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}

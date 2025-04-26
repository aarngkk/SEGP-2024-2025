using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{   // Reference to the volume slider UI element
    [SerializeField] Slider volumeSlider;

    void Start()
    {   // Check if the user has a saved volume preference
        if (!PlayerPrefs.HasKey("musicVolume"))
        {   // If not, set default volume to maximum (1)
            PlayerPrefs.SetFloat("musicVolume", 1);
        }

        else
        {   // If yes, load the saved volume setting
            Load();
        }
    }
    // Method called when the user changes the volume using the slider
    public void ChangeVolume()
    {// Update the global audio listener's volume to match the slider
        AudioListener.volume = volumeSlider.value;
        Save();
    }
    // Loads the saved volume from PlayerPrefs
    private void Load()
    {   // Set the slider value and AudioListener volume to the saved value
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        AudioListener.volume = volumeSlider.value;
    }
    // Saves the current volume to PlayerPrefs
    private void Save()
    {   // Store the current slider value as the new "musicVolume" setting
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        PlayerPrefs.Save();
    }
}

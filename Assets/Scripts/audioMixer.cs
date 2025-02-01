using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicAndSFXSlider : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer; // Reference to the Audio Mixer

    [Header("Volume Parameters")]
    [SerializeField] private string musicVolumeParameter = "MusicVolume"; // Exposed parameter for music
    [SerializeField] private string sfxVolumeParameter = "SFXVolume"; // Exposed parameter for SFX

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider; // Slider for music volume
    [SerializeField] private Slider sfxSlider; // Slider for SFX volume

    private void Start()
    {
        // Load saved volume settings
        float savedMusicVolume = PlayerPrefs.GetFloat(musicVolumeParameter, 0.75f); // Default to 75% volume
        float savedSFXVolume = PlayerPrefs.GetFloat(sfxVolumeParameter, 0.75f); // Default to 75% volume

        // Apply saved volumes
        SetVolume(musicVolumeParameter, savedMusicVolume);
        SetVolume(sfxVolumeParameter, savedSFXVolume);

        // Update slider values
        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSFXVolume;

        // Add listeners to the sliders
        musicSlider.onValueChanged.AddListener((value) => HandleVolumeChange(musicVolumeParameter, value));
        sfxSlider.onValueChanged.AddListener((value) => HandleVolumeChange(sfxVolumeParameter, value));
    }

    private void HandleVolumeChange(string parameter, float value)
    {
        SetVolume(parameter, value); // Update the volume when the slider changes
    }

    private void SetVolume(string parameter, float value)
    {
        // Convert the slider value (0 to 1) to a logarithmic scale for the Audio Mixer
        float volume = Mathf.Log10(value) * 20;
        if (value <= 0) volume = -80; // Mute if the value is 0 or below

        // Set the volume in the Audio Mixer
        audioMixer.SetFloat(parameter, volume);

        // Save the volume setting
        PlayerPrefs.SetFloat(parameter, value);
        PlayerPrefs.Save();
    }
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicAndSFXToggleButtons : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer; // Reference to the Audio Mixer

    [Header("Volume Parameters")]
    [SerializeField] private string musicVolumeParameter = "MusicVolume"; // Exposed parameter for music
    [SerializeField] private string sfxVolumeParameter = "SFXVolume"; // Exposed parameter for SFX

    [Header("Music Toggle Button")]
    [SerializeField] private Button musicToggleButton; // Button for toggling music
    [SerializeField] private Image musicButtonImage; // Image component of the music button
    [SerializeField] private Sprite musicOnSprite; // Sprite for music ON state
    [SerializeField] private Sprite musicOffSprite; // Sprite for music OFF state

    [Header("SFX Toggle Button")]
    [SerializeField] private Button sfxToggleButton; // Button for toggling SFX
    [SerializeField] private Image sfxButtonImage; // Image component of the SFX button
    [SerializeField] private Sprite sfxOnSprite; // Sprite for SFX ON state
    [SerializeField] private Sprite sfxOffSprite; // Sprite for SFX OFF state

    private bool isMusicMuted = false; // Track music mute state
    private bool isSFXMuted = false; // Track SFX mute state

    private float savedMusicVolume; // Store the music volume before muting
    private float savedSFXVolume; // Store the SFX volume before muting

    private void Start()
    {
        // Load saved mute states
        isMusicMuted = PlayerPrefs.GetInt("MusicMuteState", 0) == 1;
        isSFXMuted = PlayerPrefs.GetInt("SFXMuteState", 0) == 1;

        // Set initial button images
        musicButtonImage.sprite = isMusicMuted ? musicOffSprite : musicOnSprite;
        sfxButtonImage.sprite = isSFXMuted ? sfxOffSprite : sfxOnSprite;

        // Add listeners to the buttons
        musicToggleButton.onClick.AddListener(ToggleMusic);
        sfxToggleButton.onClick.AddListener(ToggleSFX);
    }

    private void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted; // Toggle mute state
        HandleMute(musicVolumeParameter, isMusicMuted, ref savedMusicVolume);

        // Update button image
        musicButtonImage.sprite = isMusicMuted ? musicOffSprite : musicOnSprite;

        // Save mute state
        PlayerPrefs.SetInt("MusicMuteState", isMusicMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted; // Toggle mute state
        HandleMute(sfxVolumeParameter, isSFXMuted, ref savedSFXVolume);

        // Update button image
        sfxButtonImage.sprite = isSFXMuted ? sfxOffSprite : sfxOnSprite;

        // Save mute state
        PlayerPrefs.SetInt("SFXMuteState", isSFXMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void HandleMute(string parameter, bool isMuted, ref float savedVolume)
    {
        if (isMuted)
        {
            // Save the current volume and mute
            audioMixer.GetFloat(parameter, out savedVolume);
            audioMixer.SetFloat(parameter, -80); // Mute
        }
        else
        {
            // Restore the saved volume
            audioMixer.SetFloat(parameter, savedVolume);
        }
    }
}
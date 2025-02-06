using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private float sfxVolume;
    private float musicVolume;
    [SerializeField] private GameObject SettingsUI;
    [SerializeField] private Slider musicSldier;
    [SerializeField] private Slider sfxSldier;
    private musicManager mM;
    [SerializeField] private AudioSource a_playButton;

    private void Start()
    {
        mM = musicManager.Instance;
        sfxVolume = PlayerPrefs.GetFloat("sfxVol", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("musicVol", 0.5f);

        musicSldier.value = musicVolume;
        sfxSldier.value = sfxVolume;
    }

    public void OnPlay()
    {
        a_playButton.Play();
        Invoke("CommitPlay", .5f);
    }

    private void CommitPlay()
    { 
        SceneManager.LoadScene("LevelSelection");
    }

    public void sfxSlider(float vol)
    {
        mM.sfxVolume = vol;
        PlayerPrefs.SetFloat("sfxVol",vol);
    }
    public void musicSlider(float vol)
    {
        mM.musicVolume = vol;
        PlayerPrefs.SetFloat("musicVol", vol);
    }

    public void Settings()
    {
        mM.PlayOnceClip("buttonClick");
        Time.timeScale = 0f;
        SettingsUI.SetActive(true);
    }


    public void Cancel()
    {
        mM.PlayOnceClip("buttonClick");
        Time.timeScale = 1f;
        SettingsUI.SetActive(false);
    }
}

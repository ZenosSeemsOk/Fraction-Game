using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TutorialGameManager : MonoBehaviour
{
    [SerializeField] private AudioSource a_source;
    private float sfxVolume;
    private float musicVolume;
    [SerializeField] private Slider musicSldier;
    [SerializeField] private Slider sfxSldier;
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject transparent;
    [SerializeField] private GameObject SettingsUI;
    [SerializeField] private GameObject victoryCard;
    private musicManager mM;
    private CardSpawner spawner;
    private GameManager gm;
    public static TutorialGameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1;
        mM = musicManager.Instance;
        gm = GameManager.instance;
        spawner = CardSpawner.Instance;
    }
    public void NextButton()
    {
        StartCoroutine(WaitAndLoadNextScene());
    }

    private IEnumerator WaitAndLoadNextScene()
    {
        a_source.Play();
        yield return new WaitForSeconds(a_source.clip.length);

        if (gm.totalLevelUnlocked < 20)
        {
            if (gm.levelindex >= 0 && gm.levelindex < gm.levelunlocked.Length)
            {
                if (!gm.levelunlocked[gm.levelindex])
                {
                    gm.levelunlocked[gm.levelindex] = true;
                    gm.totalLevelUnlocked += 1;
                }
                else
                {
                    Debug.Log("Level Already Cleared");
                }
            }
        }

        SceneManager.LoadScene("Arcade Mode");
    }


    public void Restart()
    {
        mM.PlayOnceClip("buttonClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Home()
    {
        mM.PlayOnceClip("buttonClick");
        SceneManager.LoadScene("LevelSelection");
    }

    public void Pause()
    {
        mM.PlayOnceClip("buttonClick");
        Time.timeScale = 0f;
        PauseUI.SetActive(true);
    }

    public void Cancel()
    {
        mM.PlayOnceClip("buttonClick");
        Time.timeScale = 1f;
        PauseUI.SetActive(false);
        SettingsUI.SetActive(false);
    }


    public void sfxSlider(float vol)
    {
        mM.sfxVolume = vol;
        PlayerPrefs.SetFloat("sfxVol", vol);
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
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SubModeGameManager : MonoBehaviour
{
    private float sfxVolume;
    private float musicVolume;
    [SerializeField] private Slider musicSldier;
    [SerializeField] private Slider sfxSldier;
    [SerializeField] private AudioSource a_source;
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private AudioSource a_Level;
    [SerializeField] private GameObject SettingsUI;
    [SerializeField] private Aircraft[] aircraft;
    [SerializeField] private GameObject[] antiAircraftLaunchers;
    [SerializeField] private RectTransform spawnPoint;
    [SerializeField] private GameObject transparent;
    [SerializeField] private GameObject victoryCard;
    public bool checkGameOver;
    private LevelSelection levelSelection;
    private GameManager gm;
    private musicManager mM;
    public static SubModeGameManager instance;
    GameObject new_Aircraft;
    GameObject new_Aircraft2;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        transparent.SetActive(false);
        mM = musicManager.Instance;
        gm = GameManager.instance;
        levelSelection = LevelSelection.Instance;
        Debug.Log(levelSelection.checkLevelIndex);

        // Spawn anti-aircraft launcher based on level index

        if (gm.levelindex > 2)
        {
             new_Aircraft2 = Instantiate(antiAircraftLaunchers[0], spawnPoint);
        }
        else
        {
             new_Aircraft = Instantiate(antiAircraftLaunchers[1], spawnPoint);
        }
    }

    private void Update()
    {
        if (checkGameOver)
        {
            checkGameOver = false; // Prevent multiple coroutines
            StartCoroutine(CheckGameOverWithDelay());
        }
    }

    private IEnumerator CheckGameOverWithDelay()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);
        if (gm.levelindex > 2)
        {
            new_Aircraft2.transform.GetChild(0).GetComponent<AntiAircraftLauncher>().enabled = false;
        }
        else
        {
            new_Aircraft.GetComponent<AntiAircraftLauncher>().enabled = false;
        }
        mM.PlayOnceClip("levelComplete");
        transparent.SetActive(true);
        victoryCard.SetActive(true);
    }

    public void NextButton()
    {
        StartCoroutine(WaitAndLoadLevelSelection());
    }

    private IEnumerator WaitAndLoadLevelSelection()
    {
        mM.PlayOnceClip("buttonClick");
        yield return new WaitForSeconds(a_source.clip.length);

        if (gm.totalLevelUnlocked < 20)
        {
            int levelToUnlock = gm.levelindex + 12;

            if (levelToUnlock < gm.levelunlocked.Length)
            {
                if (!gm.levelunlocked[levelToUnlock])
                {
                    gm.levelunlocked[levelToUnlock] = true;
                    gm.totalLevelUnlocked += 1;
                }
                else
                {
                    Debug.Log("Level Already cleared");
                }
            }
            else
            {
                Debug.LogError("Level to unlock is out of bounds!");
            }
        }

        SceneManager.LoadScene("LevelSelection");
    }

    public void Restart()
    {
        mM.PlayOnceClip("buttonClick");
        SceneManager.LoadScene("Sub-Mode 2");
    }

    public void Home()
    {
        mM.PlayOnceClip("buttonClick");
        SceneManager.LoadScene("LevelSelection");
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
    public void Pause()
    {
        mM.PlayOnceClip("buttonClick");
        Time.timeScale = 0f;
        if (gm.levelindex > 2)
        {
            new_Aircraft2.transform.GetChild(0).GetComponent<AntiAircraftLauncher>().enabled = false;
        }
        else
        {
            new_Aircraft.GetComponent<AntiAircraftLauncher>().enabled = false;
        }
        PauseUI.SetActive(true);
    }

    public void Settings()
    {
        mM.PlayOnceClip("buttonClick");
        Time.timeScale = 0f;
        SettingsUI.SetActive(true);
    }

    public void Cancel()
    {
        Time.timeScale = 1f;
        if (gm.levelindex > 2)
        {
            new_Aircraft2.transform.GetChild(0).GetComponent<AntiAircraftLauncher>().enabled = true;
        }
        else
        {
            new_Aircraft.GetComponent<AntiAircraftLauncher>().enabled = true;
        }
        PauseUI.SetActive(false);
        SettingsUI.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SubModeGameManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject SettingsUI;
    [SerializeField] private Aircraft[] aircraft;
    [SerializeField] private GameObject[] antiAircraftLaunchers;
    [SerializeField] private RectTransform spawnPoint;
    [SerializeField] private GameObject transparent;
    [SerializeField] private GameObject victoryCard;
    public bool checkGameOver;
    private LevelSelection levelSelection;
    private GameManager gm;
    public static SubModeGameManager instance;
    GameObject new_Aircraft;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        transparent.SetActive(false);
        gm = GameManager.instance;
        levelSelection = LevelSelection.Instance;
        Debug.Log(levelSelection.checkLevelIndex);

        // Spawn anti-aircraft launcher based on level index

        if (gm.levelindex > 2)
        {
             new_Aircraft = Instantiate(antiAircraftLaunchers[0], spawnPoint);
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
        new_Aircraft.GetComponent<AntiAircraftLauncher>().enabled = false;
        transparent.SetActive(true);
        victoryCard.SetActive(true);
    }

    public void NextButton()
    {
        // Only proceed if total levels unlocked haven't exceeded the cap
        if (gm.totalLevelUnlocked < 20)
        {
            // Calculate the level to unlock (current level index + 12)
            int levelToUnlock = gm.levelindex + 12;

            // Ensure the level to unlock is within bounds
            if (levelToUnlock < gm.levelunlocked.Length)
            {
                // Unlock the level if it's not already unlocked
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

        // Load the scene AFTER processing unlocks
        SceneManager.LoadScene("LevelSelection");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Sub-Mode 2");
    }

    public void Home()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        new_Aircraft.GetComponent<AntiAircraftLauncher>().enabled = false;
        PauseUI.SetActive(true);
    }

    public void Settings()
    {
        Time.timeScale = 0f;
        SettingsUI.SetActive(true);
    }

    public void Cancel()
    {
        Time.timeScale = 1f;
        new_Aircraft.GetComponent<AntiAircraftLauncher>().enabled = true;
        PauseUI.SetActive(false);
        SettingsUI.SetActive(false);
    }
}
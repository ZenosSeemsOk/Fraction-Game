using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

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
        if (gm.levelindex > 2)
        {
            Instantiate(antiAircraftLaunchers[0], spawnPoint);
        }
        else
        {
            Instantiate(antiAircraftLaunchers[1], spawnPoint);
        }

        // Start the delayed game over check

    }

    private void Update()
    {
        if (checkGameOver)
        {
            StartCoroutine(CheckGameOverWithDelay());

        }
    }

    private IEnumerator CheckGameOverWithDelay()
    {
        // Wait for 1.5 seconds
        yield return new WaitForSeconds(1f);
        transparent.SetActive(true);
        victoryCard.SetActive(true);
        // After delay, set checkGameOver to true (or perform whatever action triggers victory)

    }

    public void NextButton()
    {
        if (gm.totalLevelUnlocked < 20)
        {
            gm.totalLevelUnlocked += 1;
        }

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
        PauseUI.SetActive(false);
        SettingsUI.SetActive(false);
    }
}

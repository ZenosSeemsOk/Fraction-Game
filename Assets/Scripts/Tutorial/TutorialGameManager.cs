using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TutorialGameManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject transparent;
    [SerializeField] private GameObject SettingsUI;
    [SerializeField] private GameObject victoryCard;
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
        gm = GameManager.instance;
        spawner = CardSpawner.Instance;
    }
    public void NextButton()
    {
        SceneManager.LoadScene("Arcade Mode");
        if(gm.totalLevelUnlocked < 20)
        {
            gm.totalLevelUnlocked += 1;
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public void Cancel()
    {
        Time.timeScale = 1f;
        PauseUI.SetActive(false);
        SettingsUI.SetActive(false);
    }

    public void Settings()
    {
        Time.timeScale = 0f;
        SettingsUI.SetActive(true);
    }
}

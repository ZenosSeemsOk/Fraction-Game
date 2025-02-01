using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeSceneLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject SettingsUI;
    private CardSpawner spawner;
    private GameManager gm;

    private void Start()
    {
        Time.timeScale = 1f;
        gm = GameManager.instance;
        spawner = CardSpawner.Instance;
    }

    public void NextButton()
    {
        Debug.Log("Next Pressed");
        gm.totalLevelUnlocked += 1;
        SceneManager.LoadScene("Sub-Mode 2");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Arcade Mode");
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

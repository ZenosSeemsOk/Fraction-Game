using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeSceneLevelManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject SettingsUI;
    [SerializeField] private Transform cardParent;
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
        // Only proceed if total levels unlocked haven't exceeded the cap
        if (gm.totalLevelUnlocked < 20)
        {
            // Calculate the level to unlock (current level index + 6)
            int levelToUnlock = gm.levelindex + 6;

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
        SceneManager.LoadScene("Sub-Mode 2");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Arcade Mode");
    }

    public void Home()
    {
        Debug.Log("Clicked");
        SceneManager.LoadScene("LevelSelection");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        foreach (Transform child in cardParent)
        {
            child.GetComponent<DragDrop2D>().enabled = false;
        }
        PauseUI.SetActive(true);
    }

    public void Cancel()
    {
        Time.timeScale = 1f;
        foreach (Transform child in cardParent)
        {
            child.GetComponent<DragDrop2D>().enabled = true;
        }
        PauseUI.SetActive(false);
        SettingsUI.SetActive(false);
    }

    public void Settings()
    {
        Time.timeScale = 0f;
        foreach (Transform child in cardParent)
        {
            child.GetComponent<DragDrop2D>().enabled = false;
        }
        SettingsUI.SetActive(true);
    }
}
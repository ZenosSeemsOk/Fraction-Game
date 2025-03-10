using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    private float sfxVolume;
    private float musicVolume;
    private GameManager gm;
    public int checkLevelIndex;
    public static LevelSelection Instance;
    private musicManager mM;
    [SerializeField] private Slider musicSldier;
    [SerializeField] private Slider sfxSldier;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject menubackButton;
    [SerializeField] private GameObject levelbackButton;
    [SerializeField] private GameObject SettingsUI;
    [SerializeField] private Button[] mainButtons;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject subPanel;
    [SerializeField] private Transform[] subButtonsParents;
    [SerializeField] private GameObject[] subPanelParents;
    private Button[][] subButtons;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        mM = musicManager.Instance;
        gm = GameManager.instance;
        if(SceneManager.GetActiveScene().name == "LevelSelection")
        {
            InitializeSubButtonsArray();
            UpdateButtons();
            levelbackButton.SetActive(false);
            menubackButton.SetActive(true);
        }

    }

    private void InitializeSubButtonsArray()
    {
        subButtons = new Button[subButtonsParents.Length][];
        for (int i = 0; i < subButtonsParents.Length; i++)
        {
            Button[] buttons = subButtonsParents[i].GetComponentsInChildren<Button>();
            if (buttons.Length != 3)
            {
                Debug.LogError($"Level {i + 1} has an incorrect number of game mode buttons!");
            }
            subButtons[i] = buttons;
        }
    }

    private void UpdateButtons()
    {
        if (SceneManager.GetActiveScene().name == "LevelSelection")
        {
            // Disable all level buttons and sub-buttons initially
            foreach (Button btn in mainButtons)
            {
                btn.interactable = false;
            }
            foreach (Button[] subLevel in subButtons)
            {
                foreach (Button btn in subLevel)
                {
                    btn.interactable = false;
                }
            }
            //Enable panels based on selected

            // Enable levels and sublevels based on totalLevelUnlocked
            int levelsUnlocked = Mathf.CeilToInt(gm.totalLevelUnlocked / 3f);
            UnlockMainButtons(levelsUnlocked);

            for (int i = 0; i < levelsUnlocked; i++)
            {
                ActivateSubButtons(i);
            }
        }

    }

    private void UnlockMainButtons(int levelsUnlocked)
    {
        for (int i = 0; i < levelsUnlocked; i++)
        {
            mainButtons[i].interactable = true;
        }
    }

    public void OpenSubPanel(int levelIndex)
    {
        mM.PlayOnceClip("levelClick");
        gm.levelindex = levelIndex;
        checkLevelIndex = levelIndex;
        // Hide the main panel and show the sub-panel
        mainPanel.SetActive(false);
        subPanel.SetActive(true);
        menubackButton.SetActive(false);
        levelbackButton.SetActive(true);
        // Activate the sub-buttons for the specific level
        ActivateSubButtons(levelIndex);

        // Disable all sub-panel parents
        foreach (GameObject panel in subPanelParents)
        {
            panel.SetActive(false);
        }

        // Activate the sub-panel based on the level index (ensure it's within bounds)
        if (levelIndex >= 0 && levelIndex < subPanelParents.Length)
        {
            subPanelParents[levelIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("Invalid level index for sub-panel: " + levelIndex);
        }
    }

    private bool isSceneTransitioning = false;

    public void ActivateSubButtons(int levelIndex)
    {
        // Ensure all sub-buttons are disabled initially
        foreach (Button btn in subButtons[levelIndex])
        {
            btn.interactable = false;
        }

        // Unlock Game Mode 1 (always enabled by default)
        subButtons[levelIndex][0].interactable = true;

        // Unlock additional game modes if applicable
        int totalSubLevelsUnlockedForLevel = Mathf.Min(
            gm.totalLevelUnlocked - (levelIndex * 3),
            3
        );

        for (int i = 0; i < totalSubLevelsUnlockedForLevel; i++)
        {
            subButtons[levelIndex][i].interactable = true;
        }

        // Add listeners for each sub-button to load the respective scene
        if (subButtons[levelIndex].Length > 0)
        {
            subButtons[levelIndex][0].onClick.AddListener(() => PlayAudioAndLoadScene("Tutorial"));
        }
        if (subButtons[levelIndex].Length > 1)
        {
            subButtons[levelIndex][1].onClick.AddListener(() => PlayAudioAndLoadScene("Arcade mode"));
        }
        if (subButtons[levelIndex].Length > 2)
        {
            subButtons[levelIndex][2].onClick.AddListener(() => PlayAudioAndLoadScene("Sub-mode 2"));
        }
    }

    // Method to play audio and load the scene when finished
    private void PlayAudioAndLoadScene(string sceneName)
    {
        if (!isSceneTransitioning)
        {
            isSceneTransitioning = true;
            mM.PlayOnceClip("subLevelClick");
            SceneManager.LoadScene(sceneName);
            // Wait for the audio to finish before loading the scene
            isSceneTransitioning = false;
        }
    }

    public void Cancel()
    {
        mM.PlayOnceClip("buttonClick");
        Time.timeScale = 1f;
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


    public void BackToMainPanel()
    {
        mM.PlayOnceClip("buttonClick");
        isSceneTransitioning = false;
        menubackButton.SetActive(true);
        levelbackButton.SetActive(false);
        subPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void Settings()
    {
        mM.PlayOnceClip("buttonClick");
        Time.timeScale = 0f;
        SettingsUI.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mM.PlayOnceClip("buttonClick");
        SceneManager.LoadScene("MainMenu");
    }

}

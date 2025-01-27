using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Build.Content;

public class CardSpawner : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private GameObject[] scalePrefab; // Array of scale prefabs for different divisions
    [SerializeField] private GameObject cardPrefab; // Prefab for cards
    [SerializeField] private Transform parentTransform; // Parent object for spawned cards
    [SerializeField] private Transform[] spawnPoints; // Spawn points for cards
    [SerializeField] private Sprite[] sourceImages; // Images to assign to cards
    public int numberOfCards = 15;
    public int snapCount;
    public int scaleDivisions = 7; // Default value is 11 for level 1
    public static CardSpawner Instance { get; private set; }
    public UnityEvent OnSnapped = new UnityEvent();
    private GameManager gm;
    private LevelSelection levelSelection;

    //[SerializeField] private SubLevelCompletionManager subLevelCompletionManager; // Reference to SubLevelCompletionManager

    private int previousScaleDivisions; // To track changes in scale divisions

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        levelSelection = LevelSelection.Instance;
        gm = GameManager.instance;
        if (gm == null)
        {
            Debug.LogError("GameManager instance is not assigned.");
            return;
        }
        Debug.Log("GameManager instance found.");
        switch (levelSelection.checkLevelIndex)
        {
            case 1:
                scaleDivisions = 8;
                break;
            case 2:
                scaleDivisions = 9;
                break;
            case 3:
                scaleDivisions = 10;
                break;
            case 4:
                scaleDivisions = 11;
                break;
            case 5:
                scaleDivisions = 12;
                break;
            default:
                scaleDivisions = 7;
                break;
        }

        SetScale();

        // Spawn the cards
        SpawnCards();

        // Validate SubLevelCompletionManager reference
    }

    private void Update()
    {
        // Check if scaleDivisions has changed, and update the scale if needed
        if (scaleDivisions != previousScaleDivisions)
        {
            SetScale();
        }
    }

    private void SetScale()
    {
        foreach (var scale in scalePrefab)
        {
            scale.SetActive(false); // Deactivate all scale prefabs
        }

        switch (scaleDivisions)
        {
            case 7:
                scalePrefab[0].SetActive(true);
                break;
            case 8:
                scalePrefab[1].SetActive(true);
                break;
            case 9:
                scalePrefab[2].SetActive(true);
                break;
            case 10:
                scalePrefab[3].SetActive(true);
                break;
            case 11:
                scalePrefab[4].SetActive(true);
                break;
            case 12:
                scalePrefab[5].SetActive(true);
                break;
            default:
                Debug.LogError("Invalid scale divisions: " + scaleDivisions);
                break;
        }

        previousScaleDivisions = scaleDivisions;
    }


    private void SpawnCards()
    {
        if (spawnPoints.Length < numberOfCards || sourceImages.Length < numberOfCards)
        {
            Debug.LogError("Not enough spawn points or source images for the number of cards.");
            return;
        }

        for (int i = 0; i < numberOfCards; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            GameObject newCard = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity, parentTransform);
            AssignImageToCard(newCard, i);
            AssignFractionToCard(newCard, i + 1);
        }
    }

    private void AssignImageToCard(GameObject card, int index)
    {
        Image cardImage = card.GetComponentInChildren<Image>();
        if (cardImage != null)
        {
            cardImage.sprite = sourceImages[index];
        }
        else
        {
            Debug.LogWarning("No Image component found on the card prefab.");
        }
    }

    private void AssignFractionToCard(GameObject card, int cardIndex)
    {
        TextMeshProUGUI textMesh = card.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            int numerator = Random.Range(1, 5);
            int denominator = Random.Range(2, 6);
            float fraction = (float)numerator / denominator;
            textMesh.text = numerator + "/" + denominator;

            DragDrop2D dragDrop = card.GetComponent<DragDrop2D>();
            SnapToPosition snapToPosition = card.GetComponent<SnapToPosition>();
            if (dragDrop != null) dragDrop.value = fraction;
            if (snapToPosition != null) snapToPosition.value = fraction;
        }
    }

    public void CheckGameOver()
    {
        if (snapCount == 15)
        {
            gm.totalLevelUnlocked += 1;
        }
    }
}

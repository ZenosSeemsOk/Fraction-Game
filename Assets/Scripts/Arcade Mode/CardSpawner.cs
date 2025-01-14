using UnityEngine;
using TMPro;

public class CardSpawner : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private GameObject cardPrefab; // The card prefab
    [SerializeField] private Transform parentTransform; // Parent object to organize cards in the hierarchy
    [SerializeField] private Transform[] spawnPoints; // Array of predefined spawn points
    public int numberOfCards = 15; // Total cards to spawn
    public int snapCount;
    public static CardSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SpawnCards();
    }

    private void SpawnCards()
    {
        // Ensure the number of spawn points is sufficient
        if (spawnPoints.Length < numberOfCards)
        {
            Debug.LogError("Not enough spawn points for the number of cards to spawn.");
            return;
        }

        for (int i = 0; i < numberOfCards; i++)
        {
            // Get the spawn position from the spawnPoints array
            Transform spawnPoint = spawnPoints[i];

            // Instantiate the card at the spawn point
            GameObject newCard = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity, parentTransform);

            // Assign a fraction value to the card
            AssignFractionToCard(newCard, i + 1);
        }
    }

    private void AssignFractionToCard(GameObject card, int cardIndex)
    {
        // Get the TextMeshProUGUI component from the card prefab
        TextMeshProUGUI textMesh = card.GetComponentInChildren<TextMeshProUGUI>();

        if (textMesh != null)
        {
            // Generate a random fraction for the card
            int numerator = Random.Range(1, 5);
            int denominator = Random.Range(2, 6);
            float fraction = (float)numerator / denominator;

            // Assign the fraction as text
            textMesh.text = numerator + "/" + denominator;

            // Optionally: Assign the fraction value to a card script (if needed)
            DragDrop2D dragDrop = card.GetComponent<DragDrop2D>();
            SnapToPosition snapToPosition = card.GetComponent<SnapToPosition>();

            if (dragDrop != null) dragDrop.value = fraction;
            if (snapToPosition != null) snapToPosition.value = fraction;
        }
    }

    public void CheckGameOver()
    {
        if(snapCount == numberOfCards)
        {
            Debug.Log("Game Over");
        }
    }
}

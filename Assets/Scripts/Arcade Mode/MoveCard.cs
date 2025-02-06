using UnityEngine;

public class MoveCard : MonoBehaviour
{
    [SerializeField] private Transform cardParent;
    [SerializeField] private GameObject handPrefab;
    [SerializeField] private SnapToPosition[] scalePoints;
    private GameManager gm;
    private CardSpawner spawner;
    private musicManager mM;
    private void Start()
    {
        gm = GameManager.instance;
        mM = musicManager.Instance;
        spawner = CardSpawner.Instance;
        if (gm.levelindex == 0 && !gm.levelunlocked[0])
        {
            Invoke("CardMove", .5f);
        }
        else
        {
            this.enabled = false;
        }
    }

    private void CardMove()
    {
        // Check if cardParent has enough children
        if (cardParent.childCount < 15)
        {
            Debug.LogError("CardParent doesn't have 15 children!");
            return;
        }

        Transform card15Transform = cardParent.GetChild(14);
        GameObject card15 = card15Transform.gameObject;

        // Get the SnapToPosition component and check if it exists
        DragDrop2D cardSnap = card15.GetComponent<DragDrop2D>();
        if (cardSnap == null)
        {
            Debug.LogError("Card15 is missing DragDrop2D component!");
            return;
        }

        float cardValue = cardSnap.value;

        // Instantiate hand prefab as child of the card
        GameObject new_hand = Instantiate(handPrefab, card15Transform);
        new_hand.SetActive(false);

        // Reset local position/rotation to match parent's coordinate system
        new_hand.transform.localPosition = Vector3.zero;
        new_hand.transform.localRotation = Quaternion.identity;

        // Search through scale points
        foreach (var position in scalePoints)
        {
            if (Mathf.Approximately(cardValue, position.value))
            {
                new_hand.SetActive(true);

                LeanTween.move(card15, position.transform.position, 2f)
                    .setDelay(1f)
                    .setOnComplete(() =>
                    {
                        mM.PlayOnceClip("puzzle_Snapped");
                        spawner.snapCount++;
                        cardSnap.isSnapped = true;
                        cardSnap.enabled = false;
                        new_hand.SetActive(false);
                        Destroy(new_hand);
                    });

                break; // Exit loop after finding match
            }
        }
    }
}
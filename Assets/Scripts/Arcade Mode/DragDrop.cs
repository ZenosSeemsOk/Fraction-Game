using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class DragDrop2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;
    private Camera mainCamera;
    private SnapToPosition snap;
    private bool isHit;
    private float snappedValue;
    public bool snapCheck;
    public int mistakeCount;
    public int lastSnapCount;
    public float value;
    CardSpawner spawner;
    private void Awake()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    private void Start()
    {
        spawner = CardSpawner.Instance;
        if (spawner != null)
        {
        spawner.OnSanpped.AddListener(WrongReset);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Calculate the offset between the object's position and the mouse click
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, mainCamera.nearClipPlane));
        offset = transform.position - new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the object's position to follow the mouse cursor in the 2D world
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, mainCamera.nearClipPlane));
        transform.position = new Vector3(worldPosition.x + offset.x, worldPosition.y + offset.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isHit)
        {
            // Get the height of the card
            float cardHeight = GetComponent<Renderer>().bounds.size.y;

            // Calculate the offset (to adjust for top alignment)
            float offsetY = cardHeight / 2;  // Offset to move from center to top

            // Position the card so that its top aligns with the snap point
            Vector3 newPosition = snap.transform.position - new Vector3(0, offsetY, 0);
            transform.position = newPosition;
            if (spawner.snapCount == spawner.numberOfCards)
            {
                spawner.CheckGameOver();
            }
            //Debug.Log("Card snapped to position: " + newPosition);
            snapCheck = true;
        }
        else
        {
            snapCheck = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Position" || other.tag == "ScalePoint")
        {
            snap = other.GetComponent<SnapToPosition>();
            spawner.snapCount++;

            if (snap.value == value && !snap.isSnapped)
            {
                isHit = true;
                Debug.Log(snap.isSnapped);
                snap.isSnapped = true;
                spawner.OnSanpped.Invoke();
                if (spawner.snapCount == spawner.numberOfCards)
                {

                }
                else
                {
                    spawner.CheckGameOver();
                }

                //Debug.Log("Snap point detected: " + snap.transform.position + " with value: " + snappedValue);
            }
            else
            {
                Debug.Log(snap.isSnapped);
                isHit = false; // If values don't match, don't allow snapping
                if (other.tag == "ScalePoint")
                {
                    mistakeCount++;
                }

                Debug.Log("Value mismatch at snap point.");
                if (mistakeCount >= 4)
                {
                    WrongCard();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "ScalePoint" || other.tag == "Position")
        {
            spawner.snapCount--;
            spawner.CheckGameOver();
            snap.isSnapped = false;
            isHit = false; // Reset when exiting snap area
                           //Debug.Log("Exited snap point area.");
        }
    }

    public void WrongCard()
    {
        Debug.Log("4 times wrong");

        // Find all objects with the tag "Position" or "ScalePoint"
        SnapToPosition[] allSnapPoints = FindObjectsByType<SnapToPosition>(FindObjectsSortMode.None);

        foreach (SnapToPosition snapPoint in allSnapPoints)
        {
            // Check if the value matches and the answer key is not already enabled
            if (snapPoint.value == value && snapPoint.answerKey != null && !snapPoint.answerKey.activeSelf)
            {
                // Enable the answerKey GameObject
                snapPoint.answerKey.SetActive(true);
                Debug.Log($"Answer key enabled for snap point with value: {snapPoint.value}");
            }
        }
    }

    public void WrongReset()
    {
        mistakeCount = 0;
    }

}

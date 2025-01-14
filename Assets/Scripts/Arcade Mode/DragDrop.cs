using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;
    private Camera mainCamera;
    private SnapToPosition snap;
    private bool isHit;
    private float snappedValue;
    public bool snapCheck;
    public float value;
    CardSpawner spawner;
    private void Awake()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    private void Start()
    {
        spawner = CardSpawner.Instance;
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
            if(spawner.snapCount == spawner.numberOfCards)
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
        if (other.tag == "Position")
        {
            snap = other.GetComponent<SnapToPosition>();
            snappedValue = snap.value;
            spawner.snapCount++;

            if (snappedValue == value && !snap.isSnapped)
            {
                Debug.Log(snap.isSnapped);
                snap.isSnapped = true;
                if (spawner.snapCount == spawner.numberOfCards)
                {

                }
                else
                {
                    spawner.CheckGameOver();
                }
                isHit = true;
                //Debug.Log("Snap point detected: " + snap.transform.position + " with value: " + snappedValue);
            }
            else
            {
                Debug.Log(snap.isSnapped);
                isHit = false; // If values don't match, don't allow snapping
                Debug.Log("Value mismatch at snap point.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Position")
        {
            spawner.snapCount--;
            spawner.CheckGameOver();
            snap.isSnapped = false;
            isHit = false; // Reset when exiting snap area
            //Debug.Log("Exited snap point area.");
        }
    }
}

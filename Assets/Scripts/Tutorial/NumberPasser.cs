using UnityEngine;
using UnityEngine.EventSystems;

public class NumberPasser : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;
    [SerializeField] LineDivider lineDivider;
    [SerializeField] private Camera mainCamera;
    private SnapToPosition snap;
    private bool isNumberHolderNumerator;
    private bool isNumberHolderDenominator;
    private bool DenominatorHolder;
    public int NumberValue;
    private Vector3 initialPos;

    private void Awake()
    {
        // If mainCamera is not set in the Inspector, assign the Main Camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        initialPos = transform.position;
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
        if ((isNumberHolderNumerator || isNumberHolderDenominator || DenominatorHolder) && snap != null)
        {
            // Get the height of the card
            float cardHeight = GetComponent<Renderer>().bounds.size.y;

            // Calculate the offset (to adjust for top alignment)
            float offsetY = cardHeight / 2;  // Offset to move from center to top

            // Position the card so that its top aligns with the snap point
            Vector3 newPosition = snap.transform.position - new Vector3(0, offsetY, 0);
            transform.position = newPosition;
            transform.position = new Vector3(transform.position.x,transform.position.y,0f);

            // Update LineDivider values
            if (isNumberHolderNumerator)
            {
                lineDivider.impNumeValue = NumberValue;
                Debug.Log($"Numerator set to {lineDivider.impNumeValue}");
            }
            else if (isNumberHolderDenominator)
            {
                lineDivider.impDenoValue = NumberValue;
                Debug.Log($"Denominator set to {lineDivider.impDenoValue}");
            }
            else if (DenominatorHolder)
            {
                lineDivider.properDenoValue = NumberValue;
                Debug.Log($"Denominator for proper fraction set to {lineDivider.properDenoValue}");
            }

        }

        else
        {
            transform.position = initialPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Number Holder Numerator"))
        {
            isNumberHolderNumerator = true;
            snap = other.GetComponent<SnapToPosition>(); // Set the snap reference
        }
        else if (other.CompareTag("Number Holder Denominator"))
        {
            isNumberHolderDenominator = true;
            snap = other.GetComponent<SnapToPosition>(); // Set the snap reference
        }
        else if (other.CompareTag("Denominator Holder"))
        {
            DenominatorHolder = true;
            snap = other.GetComponent<SnapToPosition>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Number Holder Numerator"))
        {
            isNumberHolderNumerator = false;
            snap = null; // Clear the snap reference
            transform.position = initialPos;
        }
        else if (other.CompareTag("Number Holder Denominator"))
        {
            isNumberHolderDenominator = false;
            snap = null; // Clear the snap reference
            transform.position = initialPos;
        }
        else if (other.CompareTag("Denominator Holder"))
        {
            DenominatorHolder = false;
            snap = null;
            transform.position= initialPos;
        }
    }
}

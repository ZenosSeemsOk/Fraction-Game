using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class DragDrop2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;
    private Camera mainCamera;
    private SnapToPosition currentSnap;
    private bool isSnapped = false;
    public int mistakeCount;
    public float value;
    private CardSpawner spawner;
    private Vector3 originalPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
    }

    private void Start()
    {
        spawner = CardSpawner.Instance;
        if (spawner != null)
        {
            spawner.OnSnapped.AddListener(WrongReset);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, mainCamera.nearClipPlane));
        offset = transform.position - new Vector3(worldPosition.x, worldPosition.y, 0);

        if (isSnapped && currentSnap != null)
        {
            spawner.snapCount = Mathf.Max(0, spawner.snapCount - 1);
            currentSnap.isSnapped = false;
            isSnapped = false;
            spawner.CheckGameOver();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 screenBoundsMin = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 screenBoundsMax = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));

        float cardHeight = GetComponent<Renderer>().bounds.size.y;
        float cardWidth = GetComponent<Renderer>().bounds.size.x;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, mainCamera.nearClipPlane));
        Vector3 newPosition = new Vector3(
            worldPosition.x + offset.x,
            worldPosition.y + offset.y,
            0 // Ensure consistent Z-position
        );

        newPosition.x = Mathf.Clamp(newPosition.x,
            screenBoundsMin.x + cardWidth / 2,
            screenBoundsMax.x - cardWidth / 2);
        newPosition.y = Mathf.Clamp(newPosition.y,
            screenBoundsMin.y + cardHeight / 2,
            screenBoundsMax.y - cardHeight / 2);

        transform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToPosition validSnap = FindValidSnapPoint();

        if (validSnap != null)
        {
            SnapToPosition(validSnap);
        }
        else
        {
            if (!isSnapped)
            {
                mistakeCount++;
                CheckMistakes();
                StartCoroutine(MoveBackToOriginalPosition());
            }
        }
    }

    private IEnumerator MoveBackToOriginalPosition()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }

    private SnapToPosition FindValidSnapPoint()
    {
        Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(
            transform.position,
            1.0f // Increased detection radius
        );

        SnapToPosition closestSnap = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in overlappingColliders)
        {
            if (!collider.CompareTag("Position") && !collider.CompareTag("ScalePoint"))
                continue;

            SnapToPosition snap = collider.GetComponent<SnapToPosition>();
            if (snap != null && snap.value == value && !snap.isSnapped)
            {
                float distance = Vector2.Distance(transform.position, snap.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSnap = snap;
                }
            }
        }
        return closestSnap;
    }

    private void SnapToPosition(SnapToPosition snap)
    {
        if (snap == null || snap.value != value) return;

        transform.position = snap.transform.position;
        currentSnap = snap;
        currentSnap.isSnapped = true;
        isSnapped = true;
        spawner.snapCount++;
        spawner.OnSnapped.Invoke();

        if (spawner.snapCount == spawner.numberOfCards)
        {
            spawner.CheckGameOver();
        }
    }

    private void CheckMistakes()
    {
        if (mistakeCount >= 3)
        {
            WrongCard();
            StartCoroutine(ResetMistakesAfterDelay());
        }
    }

    private IEnumerator ResetMistakesAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        mistakeCount = 0;
    }

    public void WrongCard()
    {
        SnapToPosition[] allSnapPoints = FindObjectsByType<SnapToPosition>(FindObjectsSortMode.None);
        foreach (SnapToPosition snapPoint in allSnapPoints)
        {
            if (snapPoint.value == value && !snapPoint.isSnapped)
            {
                if (snapPoint.answerKey != null) snapPoint.answerKey.SetActive(true);
                if (snapPoint.glow != null) snapPoint.glow.SetActive(true);
            }
        }
    }

    public void WrongReset()
    {
        mistakeCount = 0;
        SnapToPosition[] allSnapPoints = FindObjectsByType<SnapToPosition>(FindObjectsSortMode.None);
        foreach (SnapToPosition snapPoint in allSnapPoints)
        {
            if (snapPoint.answerKey != null) snapPoint.answerKey.SetActive(false);
            if (snapPoint.glow != null) snapPoint.glow.SetActive(false);
        }
    }
}
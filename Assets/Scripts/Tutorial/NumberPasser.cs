using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class NumberPasser : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private AudioSource a_source;
    private Vector3 offset;
    private int scaleValue = 6; // Default scale value
    [SerializeField] private LineDivider lineDivider;
    private GameManager gm;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform[] resetPos;
    private SnapToPosition snap;
    private bool isNumberHolderNumerator;
    private bool isNumberHolderDenominator;
    private bool isDenominatorHolder;
    public int NumberValue;
    private Vector3 initialPos;
    private TutorialGameManager tGameManager;
    private bool isResetting = false;
    private musicManager mM;
    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Start()
    {
        // Assign initialPos based on NumberValue
        if (NumberValue > 0 && NumberValue <= resetPos.Length)
        {
            initialPos = resetPos[NumberValue - 1].position;
        }

        tGameManager = TutorialGameManager.Instance;
        gm = GameManager.instance;
        mM = musicManager.Instance;
        // Assign scaleValue based on gm.totalLevelUnlocked
        scaleValue = gm.totalLevelUnlocked switch

        {
            4 => 8,
            7 => 9,
            10 => 10,
            13 => 11,
            16 => 12,
            _ => 7
        };
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, mainCamera.nearClipPlane));
        offset = transform.position - new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, mainCamera.nearClipPlane));
        transform.position = new Vector3(worldPosition.x + offset.x, worldPosition.y + offset.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Function called");
        if ((isNumberHolderNumerator || isNumberHolderDenominator || isDenominatorHolder) && snap != null)
        {
            SnapToCorrectPosition();
            UpdateLineDividerValues();
        }
        else
        {
            StartCoroutine(SmoothReset());
        }
    }

    private void SnapToCorrectPosition()
    {
        mM.PlayOnceClip("number_Placed");
        if (snap == null) return;

        Vector3 snapPosition = snap.transform.position;
        float cardHeight = GetComponent<Renderer>().bounds.size.y;
        float offsetY = cardHeight / 2;

        transform.position = new Vector3(snapPosition.x, snapPosition.y - offsetY, 0f);
    }

    private void UpdateLineDividerValues()
    {
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
        else if (isDenominatorHolder)
        {
            lineDivider.properDenoValue = NumberValue;
            Debug.Log($"Proper fraction denominator set to {lineDivider.properDenoValue}");
        }
    }

    private IEnumerator SmoothReset()
    {
        if (isResetting) yield break;
        isResetting = true;

        float elapsedTime = 0f;
        float duration = 1f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, initialPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPos;
        isResetting = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Number Holder Numerator"))
        {
            isNumberHolderNumerator = true;
            snap = other.GetComponent<SnapToPosition>();
        }
        else if (other.CompareTag("Number Holder Denominator"))
        {
            isNumberHolderDenominator = true;
            snap = other.GetComponent<SnapToPosition>();
        }
        else if (other.CompareTag("Denominator Holder"))
        {
            isDenominatorHolder = true;
            snap = other.GetComponent<SnapToPosition>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check which trigger is exited and reset the corresponding LineDivider value
        if (other.CompareTag("Number Holder Numerator"))
        {
            lineDivider.impNumeValue = 0;
            Debug.Log("Numerator reset to 0");
        }
        else if (other.CompareTag("Number Holder Denominator"))
        {
            lineDivider.impDenoValue = 0;
            Debug.Log("Denominator reset to 0");
        }
        else if (other.CompareTag("Denominator Holder"))
        {
            lineDivider.properDenoValue = 0;
            Debug.Log("Proper denominator reset to 0");
        }

        // Reset flags and snap reference if exiting any valid holder
        if (other.CompareTag("Number Holder Numerator") || other.CompareTag("Number Holder Denominator") || other.CompareTag("Denominator Holder"))
        {
            ResetTriggerFlags();
        }
    }

    private void ResetTriggerFlags()
    {
        isNumberHolderNumerator = false;
        isNumberHolderDenominator = false;
        isDenominatorHolder = false;
        snap = null;
        StartCoroutine(SmoothReset());
    }

}

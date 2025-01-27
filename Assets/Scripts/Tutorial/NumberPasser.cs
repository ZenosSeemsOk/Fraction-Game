using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class NumberPasser : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;
    private int scaleValue = 6; // Default scale value
    [SerializeField] private LineDivider lineDivider;
    private GameManager gm;
    [SerializeField] private Camera mainCamera;
    private SnapToPosition snap;
    private bool isNumberHolderNumerator;
    private bool isNumberHolderDenominator;
    private bool isDenominatorHolder;
    public int NumberValue;
    private Vector3 initialPos;

    private bool isGameEnded = false; // Flag to track if the game has ended


    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        initialPos = transform.position;

    }

    private void Start()
    {
        gm = GameManager.instance;
        switch (gm.totalLevelUnlocked)
        {
            case 4:
                scaleValue = 8;
                break;
            case 7:
                scaleValue = 9;
                break;
            case 10:
                scaleValue = 10;
                break;
            case 13:
                scaleValue = 11;
                break;
            case 16:
                scaleValue = 12;
                break;
            default:
                scaleValue = 7;
                break;
        }
    }

    private void Update()
    {
        if (!isGameEnded) // Only check for game end if the game hasn't ended yet
        {
            CheckGameEnd();
        }
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
        if ((isNumberHolderNumerator || isNumberHolderDenominator || isDenominatorHolder) && snap != null)
        {
            SnapToCorrectPosition();
            UpdateLineDividerValues();
        }
        else
        {
            ResetPosition();
        }
    }

    private void SnapToCorrectPosition()
    {
        float cardHeight = GetComponent<Renderer>().bounds.size.y;
        float offsetY = cardHeight / 2;
        Vector3 newPosition = snap.transform.position - new Vector3(0, offsetY, 0);
        transform.position = new Vector3(newPosition.x, newPosition.y, 0f);
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

    private void ResetPosition()
    {
        transform.position = initialPos;
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
        ResetPosition();
    }

    private void CheckGameEnd()
    {
        if (lineDivider.properDenoValue == scaleValue && lineDivider.isSubmitted)
        {
            isGameEnded = true; // Mark the game as ended
            Debug.Log("Game Ended");
            OnGameEnd(); // Call the game-end logic
        }
    }

    private void OnGameEnd()
    {
        if(isGameEnded)
        {
            gm.totalLevelUnlocked += 1;
            SceneManager.LoadScene("LevelSelection");
        }
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LineDivider : MonoBehaviour
{
    public int Level;
    public int impDenoValue;
    public int impNumeValue;
    public int properDenoValue;
    public bool isSubmitted;
    [SerializeField] GameObject correctIndicator;
    [SerializeField] Transform startMarker;
    [SerializeField] Transform endMarker;
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject value1LinePrefab;
    [SerializeField] TMP_InputField denominatorField;
    [SerializeField] TextMeshProUGUI startMarkerTxt;
    [SerializeField] TextMeshProUGUI endMarkerTxt;
    private LevelSelection levelSelection;
    Vector2 size;

    [SerializeField] TMP_InputField improperDemoninatorField;
    [SerializeField] TMP_InputField improperNumeratorField;
    [SerializeField] GameObject value1Txt;
    int aircraftNumber;
    [SerializeField] GameObject aircraftPrefab;


    void Start()
    {
        levelSelection = LevelSelection.Instance;
        size = GetComponent<RectTransform>().sizeDelta;

        if (SceneManager.GetActiveScene().name == "Sub-Mode 2")
        {
            switch (levelSelection.checkLevelIndex)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    do
                    {
                        impDenoValue = Random.Range(1, 12);
                        impNumeValue = Random.Range(1, 12);
                    } while (impDenoValue <= impNumeValue); // Denominator must be greater than Numerator
                    break;
                default:
                    do
                    {
                        impDenoValue = Random.Range(1, 12);
                        impNumeValue = Random.Range(1, 12);
                    } while (impDenoValue <= impNumeValue); // Denominator must be greater than Numerator
                    break;
            }

            Debug.Log($"Denominator: {impDenoValue}, Numerator: {impNumeValue}");
            SubmitProper();
        }
    }

    public void submit()
    {
        int denominator = properDenoValue;
        if (denominator >= 10)
        {
            size.x = 1400f;
            GetComponent<RectTransform>().sizeDelta = size;
        }
        else
        {
            size.x = 1000f;
            GetComponent<RectTransform>().sizeDelta = size;

        }

        startMarkerTxt.text = $"0/{denominator}";
        endMarkerTxt.text = $"{denominator}/{denominator}";
        isSubmitted = true;
        value1Txt.SetActive(true);
        aircraftNumber = Random.Range(1, denominator + 1);
        Debug.Log(aircraftNumber);
        DivideLine(denominator);
    }

    public void SubmitProper()
    {

        int denominator = impDenoValue;
        int numerator = impNumeValue;
        if (numerator < denominator)
        {
            if (denominator >= 8)
            {
                size.x = 1600f;
                GetComponent<RectTransform>().sizeDelta = size;
            }
            else
            {
                size.x = 1000f;
                GetComponent<RectTransform>().sizeDelta = size;

            }
            value1Txt.SetActive(true);
            startMarkerTxt.text = $"0/{denominator}";
            endMarkerTxt.text = $"{denominator}/{denominator}";
            DivideLine(denominator, numerator, false);
        }
    }

    void DivideLine(int deno, int nume=0, bool isImproper=false)
    {
       
        foreach (Transform child in this.transform)
        {
            if (child != startMarker && child != endMarker)
            {
                Destroy(child.gameObject);
            }
        }

        Vector3 startPosition = startMarker.position;
        Vector3 endPosition = endMarker.position;
        int lastIndex;
        float fractionPos;

        if (!isImproper)
        {
            lastIndex=deno;
        }
        else
        {
            lastIndex= nume;
        }

        for (int i = 1; i < lastIndex; i++) // Exclude start (0) and end (1)
        {

            float fraction = (float)i / deno;
            if (!isImproper)
            {
                fractionPos = fraction;
            }
            else
            {
                fractionPos = (float)i/nume;
                Debug.Log(fractionPos);
            }
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, fractionPos);

            // Instantiate the marker prefab at the calculated position
            GameObject newMarker;
            GameObject newIndicator;
            if (i == deno && isImproper)
            {
                newMarker = Instantiate(value1LinePrefab, newPosition, Quaternion.identity, this.transform);

            }
            else
            {
                newMarker = Instantiate(linePrefab, newPosition, Quaternion.identity, this.transform);
                if (i == nume && SceneManager.GetActiveScene().name == "Tutorial")
                {
                    newIndicator = Instantiate(correctIndicator, newPosition, Quaternion.identity, this.transform);
                    if (i == aircraftNumber && Level == 2)
                    {
                        Instantiate(aircraftPrefab, newMarker.GetComponent<RectTransform>());

                    }
                }


            }

            // Set the text to display the fraction
            newMarker.GetComponentInChildren<TextMeshProUGUI>().text = $"{i}/{deno}";
            if(Level == 1)
            {
                newMarker.GetComponent<SnapToPosition>().value = (float)i / deno;
                Debug.Log((float)i / deno);
            }
        }
    }

    public void submitImproper()
    {

        int denominator = impDenoValue;
        int numerator= impNumeValue;
        if(numerator>denominator)
        {
            value1Txt.SetActive(false);
            startMarkerTxt.text = $"0/{denominator}";
            endMarkerTxt.text = $"{numerator}/{denominator}";
            DivideLine(denominator, numerator, true);
        }
    }
}

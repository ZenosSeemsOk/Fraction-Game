using UnityEngine;
using TMPro;

public class LineDivider : MonoBehaviour
{
    public int Level;
    public int impDenoValue;
    public int impNumeValue;
    public int properDenoValue;
    [SerializeField] Transform startMarker;
    [SerializeField] Transform endMarker;
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject value1LinePrefab;
    [SerializeField] TMP_InputField denominatorField;
    [SerializeField] TextMeshProUGUI startMarkerTxt;
    [SerializeField] TextMeshProUGUI endMarkerTxt;
    Vector2 size;

    [SerializeField] TMP_InputField improperDemoninatorField;
    [SerializeField] TMP_InputField improperNumeratorField;
    [SerializeField] GameObject value1Txt;
    int aircraftNumber;
    [SerializeField] GameObject aircraftPrefab;


    void Start()
    {
         size = GetComponent<RectTransform>().sizeDelta;

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
        value1Txt.SetActive(true);
        aircraftNumber = Random.Range(1, denominator + 1);
        Debug.Log(aircraftNumber);
        DivideLine(denominator);
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

            if (i == deno && isImproper)
            {
                newMarker = Instantiate(value1LinePrefab, newPosition, Quaternion.identity, this.transform);

            }
            else
            {
                newMarker = Instantiate(linePrefab, newPosition, Quaternion.identity, this.transform);
                if(i == aircraftNumber && Level==2)
                {
                    Instantiate(aircraftPrefab, newMarker.GetComponent<RectTransform>());

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

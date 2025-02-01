using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LineDivider : MonoBehaviour
{
    public int Level;
    public int impDenoValue;
    public int impNumeValue;
    public int properDenoValue;
    public bool isSubmitted;
    [SerializeField] Button pause;
    [SerializeField] Button settings;
    [SerializeField] GameObject correctIndicator;
    [SerializeField] Transform startMarker;
    [SerializeField] Transform endMarker;
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject value1LinePrefab;
    [SerializeField] TMP_InputField denominatorField;
    [SerializeField] TextMeshProUGUI startMarkerTxt;
    [SerializeField] TextMeshProUGUI endMarkerTxt;
    [SerializeField] private GameObject tutorialVictoryCard;
    private LevelSelection levelSelection;
    Vector2 size;
    [SerializeField] TextMeshProUGUI Visualfraction;

    [SerializeField] TMP_InputField improperDemoninatorField;
    [SerializeField] TMP_InputField improperNumeratorField;
    [SerializeField] GameObject value1Txt;
    private GameManager gm;
    int aircraftNumber;
    [SerializeField] GameObject[] aircraftPrefab;
    private TutorialGameManager tgm;


    void Start()
    {
        tgm = TutorialGameManager.Instance;
        gm = GameManager.instance;
        levelSelection = LevelSelection.Instance;
        size = GetComponent<RectTransform>().sizeDelta;

        if (SceneManager.GetActiveScene().name == "Sub-Mode 2")
        {
            // Define the maximum range for each level
            int maxRange = levelSelection.checkLevelIndex switch
            {
                1 => 6, // Case 1: Range 1-6
                2 => 7, // Case 2: Range 1-7
                3 => 8, // Case 3: Range 1-8
                4 => 9, // Case 4: Range 1-9
                5 => 10, // Case 5: Range 1-10
                6 => 11, // Case 6: Range 1-11
                _ => 12 // Default: Range 1-11
            };

            // Generate denominator and numerator
            do
            {
                impDenoValue = Random.Range(1, maxRange + 1); // +1 to make the range inclusive
                impNumeValue = Random.Range(1, maxRange + 1); // +1 to make the range inclusive
            } while (impDenoValue <= impNumeValue); // Ensure denominator > numerator

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
        //aircraftNumber = Random.Range(1, denominator + 1);
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
            aircraftNumber = Random.Range(1, impDenoValue - 1);
            Debug.Log($"{aircraftNumber} / {denominator}");

            if (SceneManager.GetActiveScene().name == "Sub-Mode 2")
            {
                Visualfraction.text = ($"{aircraftNumber} / {denominator}");
            }

            startMarkerTxt.text = $"0/{denominator}";
            endMarkerTxt.text = $"{denominator}/{denominator}";
            DivideLine(denominator, numerator, false);

            // Start coroutine to show victory card after 2 seconds
            if(SceneManager.GetActiveScene().name == "Tutorial")
            {
                pause.interactable = false;
                settings.interactable = false;
                StartCoroutine(ShowVictoryCardAfterDelay(2f));
            }

        }
    }

    IEnumerator ShowVictoryCardAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialVictoryCard.SetActive(true);
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

                }

                if (i == aircraftNumber && Level == 2)
                {
                    Debug.Log($"Instantiating aircraft at i={i}");
                    Debug.Log(levelSelection.checkLevelIndex);

                    // Instantiate as child of newMarker's RectTransform
                    if (gm.levelindex > 2)
                    {
                        GameObject aircraft = Instantiate(
                            aircraftPrefab[1],
                            newMarker.GetComponent<RectTransform>()
                        );
                        // Fix scale
                        aircraft.transform.localScale = new Vector3(80f, 80f, 80f); // adjusting the scale of the aircraft

                        // Adjust Z-axis position
                        aircraft.transform.localPosition = new Vector3(0, 0, -2);
                    }
                    else
                    {
                        GameObject aircraft = Instantiate(
                            aircraftPrefab[0],
                            newMarker.GetComponent<RectTransform>()
                        );
                        // Fix scale
                        aircraft.transform.localScale = new Vector3(80f, 80f, 80f); // adjusting the scale of the aircraft

                        // Adjust Z-axis position
                        aircraft.transform.localPosition = new Vector3(0, 0, -2);
                    }



                }

            }
            if(SceneManager.GetActiveScene().name == "Tutorial")
            {
                // Set the text to display the fraction
                newMarker.GetComponentInChildren<TextMeshProUGUI>().text = $"{i}/{deno}";
            }

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

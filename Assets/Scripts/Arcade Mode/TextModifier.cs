using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TextModifier : MonoBehaviour
{
    private int Deno;
    private int Nume;
    [SerializeField] private TextMeshProUGUI TextMesh_1;
    [SerializeField] private TextMeshProUGUI TextMesh_2;
    public DragDrop2D DragDrop;
    public SnapToPosition SnapToPosition;
    public int scaleDivisions = 4; // Number of divisions on the scale (e.g., quarters)
    public float Fraction;

    private static HashSet<string> usedFractions = new HashSet<string>(); // Track used fractions

    private void Start()
    {
        GenerateUniqueFraction();
    }

    private void GenerateUniqueFraction()
    {
        int attempts = 0; // To prevent infinite loops
        int maxAttempts = 20;

        do
        {
            // Base denominator based on scale divisions
            int baseDeno = scaleDivisions;

            // Generate numerator for base fraction
            int baseNume = Random.Range(1, baseDeno + 1);

            // Apply a random multiplier to create equivalent fractions
            int multiplier = Random.Range(1, 5);
            Nume = baseNume * multiplier;
            Deno = baseDeno * multiplier;

            // Create the fraction key
            string fractionKey_1 = Nume.ToString();
            string fractionKey_2 = Deno.ToString();

            // Check if the fraction has already been used
            if (!usedFractions.Contains(fractionKey_1))
            {
                // Add to used fractionss
                usedFractions.Add(fractionKey_1);

                // Calculate the fraction as a floating-point value
                Fraction = (float)Nume / Deno;

                // Update the UI text
                TextMesh_1.text = fractionKey_1;
                TextMesh_2.text = fractionKey_2;

                // Assign the fraction to DragDrop and SnapToPosition
                DragDrop.value = Fraction;
                SnapToPosition.value = Fraction;

                // Debug log for testing
                Debug.Log($"Generated Unique Fraction with numerator: {fractionKey_1} and Denominator: {fractionKey_2} for fraction: {Fraction}");
                return;
            }

            attempts++;
        } while (attempts < maxAttempts);

        // Fallback: Reset if all fractions are exhausted
        Debug.LogWarning("All unique fractions have been used. Resetting...");
        ResetUsedFractions();
        GenerateUniqueFraction();
    }

    private void ResetUsedFractions()
    {
        usedFractions.Clear();
    }
}

using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject choiceParent;

    Dictionary<string, ChoiceParent> choiceScripts;

    int voters;
    List<string> choices;

    private void Start()
    {
        voters = Data.voters;
        choices = Data.choices;
        
        voters = 3;
        choices = new List<string>{ "a", "b", "c" };

        InitialiseChoices();
    }

    void InitialiseChoices()
    {
        choiceScripts = new Dictionary<string, ChoiceParent>();

        GameObject thisChoice;
        ChoiceParent choiceScript;
        foreach (string choice in choices)
        {
            // Instantiate the GameObject
            thisChoice = Instantiate(choiceParent, content.transform);
            choiceScript = thisChoice.GetComponent<ChoiceParent>();

            // Set the text of the ChoiceParent component
            choiceScript.choiceText.text = choice;

            // Store the instantiated object in the dictionary
            choiceScripts[choice] = choiceScript;
        }
    }

    void ChangeColor(TMP_InputField scoreField, int score)
    {
        // Update the color of the score field based on the score
        // Map the score to a value between 0 (red) and 1 (green)
        float normalizedScore = Mathf.InverseLerp(-5, 5, score);
        // Interpolate between red (low) and green (high) using the normalized score
        Color baseColor = Color.Lerp(Color.red, Color.green, normalizedScore);
        // Generate lighter variations for highlighted and pressed states
        Color highlightedColor = baseColor * 1.2f; // Slightly brighter
        Color pressedColor = baseColor * 0.8f;    // Slightly darker
        // Update InputField background colors
        ColorBlock colors = scoreField.colors;
        colors.normalColor = baseColor;
        colors.highlightedColor = highlightedColor;
        colors.pressedColor = pressedColor;
        colors.selectedColor = pressedColor;
        colors.disabledColor = baseColor;
        // Assign the updated ColorBlock
        scoreField.colors = colors;
    }

    void ChangeOrder(string choice, int score)
    {
        // Rearranges the children inside the parent based on their score
    }

    void ChangeScore(string choice, int score)
    {
        // Update the score text
        TMP_InputField scoreField = choiceScripts[choice].scoreInput;
        scoreField.text = score.ToString();

        // Update the color
        ChangeColor(scoreField, score);

        // Change order in scene

        // Deactivate InputField
        scoreField.DeactivateInputField();
    }


    public void ReceiveScore(string input, string choice)
    {
        int score = int.Parse(input);
        score = Mathf.Clamp(score, -5, 5);
        ChangeScore(choice, score);
    }
}

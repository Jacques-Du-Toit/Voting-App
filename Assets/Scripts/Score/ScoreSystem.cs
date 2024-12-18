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
        
        // REMOVE AFTER
        voters = 3;
        choices = new List<string>{ "movie", "show", "book", "game", "arm-wrestle", "looooooooong optioooooooon", "first 20 mins of tomorrow i", "c", "e", "f", "g", "z", "m", "n", "hello this is a really long option to test the limits of the button" };

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

    void CalculateOrder(string choice, int score)
    {
        Transform parentPos = content.transform;
        Transform thisPos = choiceScripts[choice].gameObject.transform;

        for (int i = 0; i < parentPos.childCount; i++)
        {
            Transform childPos = parentPos.GetChild(i);
            int childScore = childPos.GetComponent<ChoiceParent>().score;

            if (childPos == thisPos || score < childScore) { continue; }
            else
            {
                thisPos.SetSiblingIndex(i - 1);
                return;
            }
        }
        thisPos.SetAsLastSibling();
    }

    void ChangeScore(string choice, int score)
    {
        // Update the score text
        TMP_InputField scoreField = choiceScripts[choice].scoreInput;
        scoreField.text = score.ToString();
        choiceScripts[choice].score = score;

        // Update the color
        ChangeColor(scoreField, score);

        // Change order in scene
        CalculateOrder(choice, score);

        // Deactivate InputField
        scoreField.DeactivateInputField();
    }


    public void ReceiveScore(string choice, string input)
    {
        int score = int.Parse(input);
        score = Mathf.Clamp(score, -5, 5);
        ChangeScore(choice, score);
    }
}

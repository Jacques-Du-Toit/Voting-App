using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour, IInputReceiver
{
    public int voters;
    public List<string> choices;

    [SerializeField] GameObject voterField;
    [SerializeField] GameObject choicesParent;

    Dictionary<string, Action<string>> inputFieldActions;
    bool isVoters = true;

    [SerializeField] TMP_Text votersText;

    [SerializeField] GameObject choiceButtonsLayout;
    [SerializeField] GameObject generalChoiceButton;
    GameObject thisChoiceButton;

    private void Start()
    {
        if (voterField == null || choicesParent == null)
        {
            Debug.LogError("Either voters or choices fields not assigned in inspector.");
        }

        inputFieldActions = new Dictionary<string, Action<string>>
        {
            { "Voter Input", HandleVoterInput },
            { "Choices Input", HandleChoicesInput }
        };
    }

    public void ReceiveInput(string inputText, GameObject inputField)
    {
        // Check if the input field action exists
        if (inputFieldActions.TryGetValue(inputField.name, out var action))
        {
            action.Invoke(inputText);
        }
        else
        {
            Debug.LogError($"{inputField.name} is not accounted for.");
        }
    }

    private void HandleVoterInput(string votersInput)
    {
        // Check for invalid input
        if (!int.TryParse(votersInput, out voters) || voters <= 0)
        {
            print($"{votersInput} is not a valid number of voters.");
            return;
        }
        // Update Voters Button
        votersText.text = $"Voters: {votersInput}";
        // Deactivate voter field and activate the choices field
        SwitchInput();
    }

    private void HandleChoicesInput(string choiceInput)
    {
        if (choiceInput != "" && !choices.Contains(choiceInput))
        {
            // Add to list of current choices
            choices.Add(choiceInput);
            // Add the choice to the layout so the user can see and remove it
            thisChoiceButton = Instantiate(generalChoiceButton, choiceButtonsLayout.transform);
            thisChoiceButton.GetComponent<ChoiceButton>().AddText(choiceInput);
        }
    }

    public void RemoveChoice(string choice)
    {
        choices.Remove(choice);
    }

    public void SwitchInput()
    {
        choicesParent.SetActive(isVoters);
        voterField.SetActive(!isVoters);
        isVoters = !isVoters;
    }
}

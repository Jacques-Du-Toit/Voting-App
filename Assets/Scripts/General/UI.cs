using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI : MonoBehaviour, IInputReceiver
{
    public int voters;
    public List<string> choices;

    [SerializeField] GameObject voterField;
    [SerializeField] GameObject choicesParent;
    [SerializeField] GameObject systemsParent;

    Dictionary<string, Action<string>> inputFieldActions;

    [SerializeField] TMP_Text votersText;

    [SerializeField] GameObject choiceButtonsLayout;
    [SerializeField] GameObject generalChoiceButton;
    [SerializeField] TMP_Text numChoicesText;

    private string ListToString(List<string> list)
    {
        // Player prefs can't store List's so we enocode the choices as a string

        // Efficiently concatenates strings using StringBuilder
        StringBuilder sb = new StringBuilder();

        foreach (string item in list)
        {
            // Append the length, a separator, and the item itself
            sb.Append(item.Length);
            sb.Append('_');
            sb.Append(item);
        }
        return sb.ToString();
    }

    private List<string> StringToList(string str)
    {
        // Turns an encoded string back into its original list
        List<string> list = new List<string>();

        int i = 0;
        string itemLength = "";
        string nextItem;

        while (i < str.Length)
        {
            if (str[i] != '_')
            {
                // Keep grabbing the next number until we hit a _
                itemLength += str[i].ToString();
                i++;
            }
            else
            {
                // When we hit a _, grab the next item and add it to the list
                i++; // Skip over the _
                nextItem = str.Substring(i, int.Parse(itemLength));
                list.Add(nextItem);
                // Skip to next number and reset itemLength
                i += int.Parse(itemLength);
                itemLength = "";
            }
        }
        return list;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Voters", voters);
        Data.voters = voters;
        PlayerPrefs.SetString("Choices", ListToString(choices));
        Data.choices = choices;
    }

    public void LoadData()
    {
        choices = StringToList(PlayerPrefs.GetString("Choices"));
        foreach (string choice in choices)
        {
            CreateChoiceButton(choice);
        }
        UpdateNumberChoicesText();
    }

    private void Start()
    {
        if (voterField == null || choicesParent == null)
        {
            Debug.LogError("Either voters or choices fields not assigned in inspector.");
        }
      
        LoadData();

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
            Debug.LogWarning($"{votersInput} is not a valid number of voters.");
            return;
        }
        // Update Voters Button
        votersText.text = $"Voters: {votersInput}";
        // Deactivate voter field and activate the choices field
        SwitchInput("choices");
    }

    private void ToggleChoicesVisibility()
    {
        // Switches whether the choices are shown or not
        bool choicesActive = choiceButtonsLayout.gameObject.activeSelf;
        choiceButtonsLayout.gameObject.SetActive(!choicesActive);
    }

    private void CreateChoiceButton(string choiceInput)
    {
        GameObject thisChoiceButton = Instantiate(generalChoiceButton, choiceButtonsLayout.transform);
        thisChoiceButton.GetComponent<ChoiceButton>().AddText(choiceInput);
    }

    private string StandardiseText(string text)
    {
        return text.ToLower().Replace(" ", "").Replace("-", "");
    }

    private bool IsValidChoice(string choiceInput, List<string> choices)
    {
        // Checks that the choice is valid/has not been entered before
        if (choiceInput == "")
        {
            return false;
        }
        foreach (string choice in choices) {
            if (StandardiseText(choiceInput) == StandardiseText(choice))
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateNumberChoicesText()
    {
        numChoicesText.text = $"{choices.Count}";
    }

    private void HandleChoicesInput(string choiceInput)
    {
        if (!IsValidChoice(choiceInput, choices))
        {
            return;
        }
        // Add to list of current choices
        choices.Add(choiceInput);
        // Add the choice to the layout so the user can see and remove it
        CreateChoiceButton(choiceInput);
        // Update the number showing the number of choices so far
        UpdateNumberChoicesText();
        SaveData();
    }

    public void RemoveChoice(string choice)
    {
        choices.Remove(choice);
        UpdateNumberChoicesText();
        SaveData();
    }

    public void SwitchInput(string layout)
    {
        voterField.SetActive(layout == "voters");
        choicesParent.SetActive(layout == "choices");
        systemsParent.SetActive(layout == "systems");
    }
}

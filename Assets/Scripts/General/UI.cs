using System;
using System.Collections.Generic;
using System.Text;
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

    private string ListToString(List<string> list)
    {
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
                print(nextItem);
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
        PlayerPrefs.SetString("Choices", ListToString(choices));
    }

    public void LoadData()
    {
        HandleVoterInput(PlayerPrefs.GetInt("Voters").ToString());
        choices = StringToList(PlayerPrefs.GetString("Choices"));

        foreach (string choice in choices)
        {
            CreateChoiceButton(choice);
        }

        print("Here");
        print(voters);
        print(ListToString(choices));
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
            print($"{votersInput} is not a valid number of voters.");
            return;
        }
        // Update Voters Button
        votersText.text = $"Voters: {votersInput}";
        // Deactivate voter field and activate the choices field
        SwitchInput();
    }

    private void CreateChoiceButton(string choiceInput)
    {
        GameObject thisChoiceButton = Instantiate(generalChoiceButton, choiceButtonsLayout.transform);
        thisChoiceButton.GetComponent<ChoiceButton>().AddText(choiceInput);
    }

    private void HandleChoicesInput(string choiceInput)
    {
        if (choiceInput == "" || choices.Contains(choiceInput))
        {
            return;
        }
        // Add to list of current choices
        choices.Add(choiceInput);
        // Add the choice to the layout so the user can see and remove it
        CreateChoiceButton(choiceInput);
        SaveData();
    }

    public void RemoveChoice(string choice)
    {
        print(ListToString(choices));
        print( StringToList(ListToString(choices)) );
        choices.Remove(choice);
        SaveData();
    }

    public void SwitchInput()
    {
        choicesParent.SetActive(isVoters);
        voterField.SetActive(!isVoters);
        isVoters = !isVoters;
    }
}

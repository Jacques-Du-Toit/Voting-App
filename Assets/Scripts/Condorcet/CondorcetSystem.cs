using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

public class CondorcetSystem : MonoBehaviour
{
    public int voters;
    public TMP_InputField votersInput;

    public List<string> choices;
    public TMP_InputField choiceInput;

    public void SetVoters()
    {
        if (!int.TryParse(votersInput.text, out voters) || voters <= 0)
        {
            print($"{votersInput.text} is not a valid integar");
            votersInput.text = "";
            votersInput.ActivateInputField();
        }
        else
        {
            votersInput.gameObject.SetActive(false);
            choiceInput.gameObject.SetActive(true);
            choiceInput.ActivateInputField();
        }
    }

    public GameObject choiceButtonsLayout;
    public GameObject generalChoiceButton;
    GameObject thisChoiceButton;
    public void AddChoice()
    {
        if (choiceInput.text != "" && !choices.Contains(choiceInput.text))
        {
            choices.Add(choiceInput.text);
            // Add the choice to the layout so the user can see and remove it
            thisChoiceButton = Instantiate(generalChoiceButton, choiceButtonsLayout.transform);
            thisChoiceButton.GetComponent<ChoiceButton>().AddText(choiceInput.text);
            // Reset the input field
            choiceInput.text = "";
        }
        choiceInput.ActivateInputField();
    }

    public void RemoveChoice(string choice)
    {
        choices.Remove(choice);
    }

    public List<List<string>> rounds = new List<List<string>>();
    List<List<string>> CreateRounds()
    {
        // Creates all unique 1v1 rounds between different choices
        for (int i = 0; i < choices.Count - 1; i++)
        {
            for (int j = i + 1; j < choices.Count; j++)
            {
                rounds.Add(new List<string> { choices[i], choices[j] });
            }
        }
        return rounds;
    }

    public GameObject condorcetRound;
    GameObject thisRound;

    public GameObject votersObject;
    GameObject thisVotersObject;

    public int voterIndex = 1;
    public int roundIndex;

    IEnumerator<WaitForSeconds> VoterRoutine(int voterIndex)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(thisVotersObject);
        ShowRound();
    }

    public void ShowVoter()
    {
        thisVotersObject = Instantiate(votersObject, this.transform);
        StartCoroutine(VoterRoutine(voterIndex));
    }

    public void CheckForNextRound()
    {
        // See's if we need to go to a new voter or the results
        if (roundIndex == rounds.Count)
        {
            voterIndex++;
            if (voterIndex == voters + 1)
            {
                Destroy(thisRound);
                Results();
            }
            else
            {
                roundIndex = 0;
                ShowVoter();
            }
        }
        else
        {
            ShowRound();
        }
    }

    public void ShowRound()
    {
        if (thisRound != null)
        {
            // Remove previous round
            Destroy(thisRound);
        }
        // Creates a rounds UI (2 buttons) based on the current round index
        thisRound = Instantiate(condorcetRound, this.transform);
        thisRound.GetComponent<CondorcetRound>().Battle(
            rounds[roundIndex][0], rounds[roundIndex][1],
            roundIndex, rounds.Count
            );
    }

    public Dictionary<string, int> votes = new Dictionary<string, int>();
    public void StartCondorcet()
    {
        if (choices.Count < 2)
        {
            print("Need at least 2 choices");
            return;
        }
        choiceInput.gameObject.SetActive(false);

        // Initialise each choice with 0 votes in a dictionary
        foreach (string choice in choices)
        {
            votes.Add(choice, 0);
        }

        rounds = CreateRounds();

        ShowVoter();
    }

    //public GameObject resultsMenu;
    public TMP_Text resultsText;
    void Results()
    {
        foreach (var entry in votes)
        {
            resultsText.text += $"{entry.Key}: {entry.Value}\n\n";
        }
        resultsText.gameObject.SetActive(true);
    }
}

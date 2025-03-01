using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class RankSystem : MonoBehaviour, IVoteSystem
{
    public string Title { get; private set; }

    [SerializeField] GameObject content;
    [SerializeField] GameObject rankChoice;

    Dictionary<string, RankChoice> choiceScripts;
    public Dictionary<string, int> ChoiceVotes { get; private set; }

    int voters;
    List<string> choices;

    private void Awake()
    {
        Title = "Rank Options";
        ChoiceVotes = new Dictionary<string, int>();
    }

    private void Start()
    {
        voters = Data.voters;
        choices = Data.choices;

        InitialiseChoices();
    }

    void InitialiseChoices()
    {
        // 1. Creates the different choice buttons
        // 2. Adds their text
        // 3. Saves their scripts into the dictionary choiceScripts
        choiceScripts = new Dictionary<string, RankChoice>();

        GameObject thisChoice;
        RankChoice choiceScript;
        foreach (string choice in choices)
        {
            // Instantiate the GameObject
            thisChoice = Instantiate(rankChoice, content.transform);
            choiceScript = thisChoice.GetComponent<RankChoice>();

            // Set the text of the ChoiceParent component
            choiceScript.choiceText.text = choice;

            // Store the instantiated object in the dictionary
            choiceScripts[choice] = choiceScript;
        }
    }
}

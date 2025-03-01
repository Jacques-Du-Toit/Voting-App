using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class RankSystem : MonoBehaviour, IVoteSystem
{
    public string Title { get; private set; }

    [SerializeField] GameObject content;
    [SerializeField] GameObject rankChoice;
    Transform parentPos;
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
        parentPos = content.transform;
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

        choices.Shuffle();
        int i = choices.Count - 1; // voters are added top to bottom so top gets highest score
        foreach (string choice in choices)
        {
            // Instantiate the GameObject
            thisChoice = Instantiate(rankChoice, content.transform);
            choiceScript = thisChoice.GetComponent<RankChoice>();

            // Set the text of the ChoiceParent component
            choiceScript.choiceText.text = choice;

            // Set the current rank of the choice in the ChoiceVotes dict
            ChoiceVotes[choiceScript.choiceText.text] = i;
            i--;

            // Store the instantiated object in the dictionary
            choiceScripts[choice] = choiceScript;
        }

        foreach (var entry in ChoiceVotes)
        {
            print($"{entry.Key}: {entry.Value}");
        }
    }

    void MoveChoice(string choice, int direction)
    {

    }

    void CalculateOrder(string choice, int score)
    {
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
}

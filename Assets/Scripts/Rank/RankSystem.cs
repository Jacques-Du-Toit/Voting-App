using UnityEngine;
using System.Collections.Generic;

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
    }

    public void MoveChoice(string choice, int direction)
    {
        // Moves a choice up or down depending on direction

        Transform thisPos = choiceScripts[choice].gameObject.transform;
        int index = thisPos.GetSiblingIndex();

        // NEED TO UPDATE THE ChoiceVotes AS WELL AND GET THE BUTTONS TO CALL THIS FUNCTION (probably from RankChoice script

        /*
         * Would need to update all the other choices as well if we wrap around
        if (index == 0 && direction == 1)
        {
            thisPos.SetAsLastSibling();
            ChoiceVotes[choice] = 0;

        }
        else if (index == (choices.Count-1) && direction == -1)
        {
            thisPos.SetAsFirstSibling();
            ChoiceVotes[choice] = choices.Count - 1;
        }
        else
        {
            thisPos.SetSiblingIndex(index + direction);
        }
        */
        if (
            (index == 0 && direction == 1) ||
            (index == (choices.Count - 1) && direction == -1)
            )
        { return; }
        ChoiceVotes[choice] += direction;
        parentPos.GetChild(index);
        thisPos.SetSiblingIndex(index - direction); // Going up decreases index
    }
}

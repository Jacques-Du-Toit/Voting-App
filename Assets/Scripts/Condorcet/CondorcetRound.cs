using UnityEngine;
using TMPro;

public class CondorcetRound : MonoBehaviour
{
    public TMP_Text roundText;
    public TMP_Text buttonOneText;
    public TMP_Text buttonTwoText;
    public void Battle(string choice1, string choice2, int roundIndex, int totalRounds)
    {
        roundText.text = $"{roundIndex + 1}/{totalRounds}";
        buttonOneText.text = choice1;
        buttonTwoText.text = choice2;
    }


    CondorcetSystem condorcetScript;

    private void Start()
    {
        condorcetScript = GameObject.FindAnyObjectByType<CondorcetSystem>();
    }

    public void VoteOnOne()
    {
        // Increments choice1's vote by 1
        condorcetScript.votes[buttonOneText.text]++;
        NextRound();
    }

    public void VoteOnTwo()
    {
        condorcetScript.votes[buttonTwoText.text]++;
        NextRound();
    }

    void NextRound()
    {
        condorcetScript.roundIndex++;
        condorcetScript.CheckForNextRound();
    }
}

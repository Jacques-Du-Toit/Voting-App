using UnityEngine;
using TMPro;

public class CondorcetRound : MonoBehaviour
{
    public TMP_Text roundText;
    public TMP_Text buttonOneText;
    public TMP_Text buttonTwoText;

    CondorcetSystem condorcetScript;

    public void Battle(string choice1, string choice2, int roundIndex, int totalRounds)
    {
        roundText.text = $"{roundIndex + 1}/{totalRounds}";
        buttonOneText.text = choice1;
        buttonTwoText.text = choice2;
    }

    private void Start()
    {
        condorcetScript = GameObject.FindAnyObjectByType<CondorcetSystem>();
    }
    
    public void VoteOnOne()
    {
        // Increments choice1's vote by 1
        UpdateVotes(buttonOneText.text);
    }

    public void VoteOnTwo()
    {
        UpdateVotes(buttonTwoText.text);
    }

    void UpdateVotes(string vote)
    {
        condorcetScript.votes[vote]++;
        condorcetScript.NextRound();
        Destroy(gameObject);
    }
}

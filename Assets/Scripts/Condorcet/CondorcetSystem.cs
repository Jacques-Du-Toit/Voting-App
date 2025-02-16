using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class CondorcetSystem : MonoBehaviour
{
    public int voters;
    public int voter;
    List<string> choices;

    List<List<string>> rounds = new List<List<string>>();
    public int roundIndex;

    [SerializeField] GameObject voterBackground;

    [SerializeField] GameObject condorcetRound;

    [SerializeField] GameObject results;
    [SerializeField] TMP_Text resultsText;
    public Dictionary<string, int> votes = new Dictionary<string, int>();

    private void Start()
    {
        voters = Data.voters;
        choices = Data.choices;

        foreach (string choice in choices)
        {
            votes[choice] = 0;
        }

        rounds = CreateRounds();
        roundIndex = rounds.Count;
        NextRound();
    }

    List<List<string>> CreateRounds()
    {
        List<List<string>> tempRounds = new List<List<string>>();
        // Creates all unique 1v1 rounds between different choices
        for (int i = 0; i < choices.Count - 1; i++)
        {
            for (int j = i + 1; j < choices.Count; j++)
            {
                tempRounds.Add(new List<string> { choices[i], choices[j] });
            }
        }
        // Randomise rounds so that we don't get the same choice compared consecutively always
        while (tempRounds.Count > 0)
        {
            int comparisonIndex = Random.Range(0, tempRounds.Count);
            rounds.Add(tempRounds[comparisonIndex]);
            tempRounds.RemoveAt(comparisonIndex);
        }
        return rounds;
    }

    public void NextRound()
    {
        GameObject thisRound;
        if (roundIndex == rounds.Count)
        {
            voter += 1;

            if (voter == voters + 1)
            {
                Results();
            }
            else
            {
                Instantiate(voterBackground, this.transform);
            }
        }
        else
        {
            thisRound = Instantiate(condorcetRound, this.transform);
            thisRound.GetComponent<CondorcetRound>().Battle(
                rounds[roundIndex][0], rounds[roundIndex][1],
                roundIndex, rounds.Count);
            roundIndex++;
        }
    }

    void Results()
    {
        foreach (var entry in votes)
        {
            resultsText.text += $"{entry.Key}: {entry.Value}\n\n";
        }
        results.SetActive(true);
    }
}

using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

public class CondorcetSystem : MonoBehaviour
{
    public int voters;
    List<string> choices;

    [SerializeField] TMP_Text resultsText;
    Dictionary<string, int> votes = new Dictionary<string, int>();

    private void Start()
    {
        voters = Data.voters;
        choices = Data.choices;
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

    void Results()
    {
        foreach (var entry in votes)
        {
            resultsText.text += $"{entry.Key}: {entry.Value}\n\n";
        }
        resultsText.gameObject.SetActive(true);
    }
}

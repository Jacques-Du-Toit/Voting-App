using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class ResultScore : MonoBehaviour
{
    [SerializeField] TMP_Text resultText;

    public void RunResults(Dictionary<string, int[]> scores)
    {
        PerChoiceResults(scores);
    }

    void PerChoiceResults(Dictionary<string, int[]> scores)
    {
        float avg;
        resultText.text += "<size=72>Per Option Results:</size>\n\n"; // Font size 72
        resultText.text += "<size=50>Averages:</size>\n\n";
        foreach (var entry in scores)
        {
            avg = Mathf.Round((float)entry.Value.Average() * 100f) / 100f;
            resultText.text += $"<size=50>{entry.Key}: {avg}</size>\n";
        }
    }
}

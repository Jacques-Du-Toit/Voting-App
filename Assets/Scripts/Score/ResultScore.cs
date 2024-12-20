using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class ResultScore : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] TMP_Text resultText;
    [SerializeField] GameObject statsTable;
    [SerializeField] GameObject statsRow;

    public void RunResults(Dictionary<string, int[]> scores)
    {
        scores = new Dictionary<string, int[]> {
            { "choice 1", new int[] { 1, 2, 3 } },
            { "choice 2", new int[] { -1, 0, 1 } },
            { "choice 3", new int[] { -4, 4, -5 } },
        };
        PerChoiceResults(scores);
    }

    void PerChoiceResults(Dictionary<string, int[]> scores)
    {
        GameObject choiceStats;
        GameObject rowStats;
        string addSep;
        float avg;
        float sumOfSquares;
        float std;
        float min;
        float max;

        // Give a title and mapping
        resultText.text += "<size=72>Per Option Results:</size>\n\n"; // Font size 72
        for (int i = 0; i < Data.choices.Count; i++)
        {
            addSep = i == Data.choices.Count - 1 ? "" : ", "; // Add ", " if another choice after
            resultText.text += $"<size=50>{i+1}: {Data.choices[i]}{addSep}</size>";
        }

        // Create the choices table
        choiceStats = Instantiate(statsTable, content.transform);

        int c = 1;
        foreach (var entry in scores)
        {
            // Add a new row to the table
            rowStats = Instantiate(statsRow, choiceStats.transform);

            // Calculate stats
            avg = Mathf.Round((float)entry.Value.Average() * 100f) / 100f;
            sumOfSquares = entry.Value.Select(val => Mathf.Pow(val - avg, 2)).Sum();
            std = Mathf.Round(Mathf.Sqrt(sumOfSquares / entry.Value.Length) * 100f) / 100f;
            min = entry.Value.Min();
            max = entry.Value.Max();

            // Send stats to the row so it can add them
            rowStats.GetComponent<Row>().AddValues(c, avg, std, min, max);
            c++;
        }
        choiceStats.GetComponent<Table>().ColorTable();
    }
}

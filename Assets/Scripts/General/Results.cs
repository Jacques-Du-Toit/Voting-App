using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Results : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject statsTable;
    [SerializeField] GameObject statsRow;
    [SerializeField] GameObject scoreDifferences;

    public void RunResults(Dictionary<string, int[]> choiceScores, Dictionary<string, int[]> voterScores)
    {
        /*
        choiceScores = new Dictionary<string, int[]>() {
            { "movie", new int[] { 3, 1, 3 } },
            { "show", new int[]  { -5, 4, -2 } },
            { "game", new int[]  { 5, -2, 1 } },
        };

        voterScores = new Dictionary<string, int[]>()
        {
            { "1", new int[] { 3, -5, 5 } },
            { "2", new int[] { 1, 4, -2 } },
            { "3", new int[] { 3, -2, 1 } },
        };
        */

        GenericTable("Option", choiceScores, true);
        GenericTable("Voter", voterScores, false);
        VoterSimilarity(voterScores);
    }

    void GenericTable(string name, Dictionary<string, int[]> indexValues, bool addMapping)
    {
        string title = "";
        GameObject thisTableStats;
        GameObject rowStats;
        float avg;
        float sumOfSquares;
        float std;
        float min;
        float max;

        // Create the table
        thisTableStats = Instantiate(statsTable, content.transform);

        // Give a title
        title += $"<size=50>Per {name} Results:</size>\n"; // Font size 72

        if (addMapping)
        {
            // Maps the strings to an index for the table as full strings are too big
            List<string> keys = new List<string>(indexValues.Keys);
            title += "<size=30>";
            for (int i = 0; i < keys.Count; i++)
            {
                title += $"{i + 1}: {keys[i]}, ";
            }
            title = title.TrimEnd(' ', ',');
            title += "</size>\n";
        }
        thisTableStats.GetComponent<Table>().SetTitle(title);

        int c = 1;
        foreach (var entry in indexValues)
        {
            // Add a new row to the table
            rowStats = Instantiate(statsRow, thisTableStats.transform);

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
        thisTableStats.GetComponent<Table>().ColorTable();
    }

    void VoterSimilarity(Dictionary<string, int[]> voterScores)
    {
        GameObject thisScoreDiff;
        thisScoreDiff = Instantiate(scoreDifferences, content.transform);
        thisScoreDiff.GetComponent<ScoreDifferences>().VoterSimilarity(voterScores);
    }
}

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Results : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject statsTable;
    [SerializeField] GameObject statsRow;
    [SerializeField] GameObject voteDifferences;

    public void RunResults(Dictionary<string, int[]> choiceVotes, Dictionary<string, int[]> voterVotes, string title)
    {
        /*
        choiceVotes = new Dictionary<string, int[]>() {
            { "movie", new int[] { 3, 1, 3 } },
            { "show", new int[]  { -5, 4, -2 } },
            { "game", new int[]  { 5, -2, 1 } },
        };

        voterVotes = new Dictionary<string, int[]>()
        {
            { "1", new int[] { 3, -5, 5 } },
            { "2", new int[] { 1, 4, -2 } },
            { "3", new int[] { 3, -2, 1 } },
        };
        */

        GenericTable("Option", choiceVotes, true);
        if (title.StartsWith("Vote"))
        {
            GenericTable("Voter", voterVotes, false);
        }
        VoterSimilarity(voterVotes);
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
            // Give the final result (who won)
            float bestAvg = -100.0f;
            float bestMin = -100.0f;

            Dictionary<string, float> choiceAverages = new Dictionary<string, float>();
            Dictionary<string, float> choiceMins = new Dictionary<string, float>();

            foreach (var entry in indexValues)
            {
                // Calculate stats
                avg = (float)entry.Value.Average();
                min = entry.Value.Min();

                if (avg > bestAvg)
                {
                    bestAvg = avg;
                }
                if (min > bestMin)
                {
                    bestMin = min;
                }

                choiceAverages[entry.Key] = avg;
                choiceMins[entry.Key] = min;
            }

            float avgRounded;
            // Display highest average
            title += $"<size=30>\nBest Average:\n";
            foreach (var entry in choiceAverages)
            {
                if (entry.Value >= bestAvg)
                {
                    avgRounded = Mathf.Round((float)entry.Value * 100f) / 100f;
                    title += $"{entry.Key}: {avgRounded}, ";
                }
            }
            title = title.TrimEnd(' ', ',');
            title += "</size>\n";

            float bestAvgForMin = -100.0f;
            // Calculate best avg for highest min
            foreach (var entry in choiceMins)
            {
                if ((entry.Value >= bestMin) && (choiceAverages[entry.Key] > bestAvgForMin))
                {
                    bestAvgForMin = choiceAverages[entry.Key];
                }
            }

            float minRounded;
            // Display highest average for highest min
            title += $"<size=30>Best Min with Best Avg:\n";
            foreach (var entry in choiceMins)
            {
                if ((entry.Value >= bestMin) && (choiceAverages[entry.Key] >= bestAvgForMin))
                {
                    avgRounded = Mathf.Round((float)entry.Value * 100f) / 100f;
                    minRounded = Mathf.Round((float)bestAvgForMin * 100f) / 100f;
                    title += $"{entry.Key}: [{avgRounded}, {minRounded}], ";
                }
            }
            title = title.TrimEnd(' ', ',');
            title += "</size>\n";

            thisTableStats.GetComponent<Table>().SetTitle(title);

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
                rowStats.GetComponent<Row>().AddValues(
                    (string)entry.Key.Substring(0, 3), avg.ToString(), std.ToString(), min.ToString(), max.ToString()
                    );
            }
            thisTableStats.GetComponent<Table>().ColorTable();
        }
    }
    void VoterSimilarity(Dictionary<string, int[]> voterVotes)
    {
        GameObject thisVoteDiff;
        thisVoteDiff = Instantiate(voteDifferences, content.transform);
        thisVoteDiff.GetComponent<VoterDifferences>().VoterSimilarity(voterVotes);
    }
}

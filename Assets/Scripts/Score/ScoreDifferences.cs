using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ScoreDifferences : MonoBehaviour
{
    [SerializeField] TMP_Text similarText;
    [SerializeField] TMP_Text differentText;

    int ArrayDifferences(int[] array1, int[] array2)
    {
        return array1.Zip(array2, (a, b) => (a - b)).Sum();
    }

    public void VoterSimilarity(Dictionary<string, int[]> voterScores)
    {
        similarText.text = "Most Similar Voters:\n";
        differentText.text = "Least Similar Voters:\n";

        if (Data.voters < 2)
        {
            return;
        }

        int diff;
        int minDiff = 100;
        int maxDiff = -100;

        // First pass to get the min and max
        for (int i = 1; i < Data.voters; i++)
        {
            for (int j = i + 1; j <= Data.voters; j++)
            {
                diff = ArrayDifferences(voterScores[i.ToString()], voterScores[j.ToString()]);
                if (diff < minDiff)
                {
                    minDiff = diff;
                }
                if (diff > maxDiff)
                {
                    maxDiff = diff;
                }
            }
        }

        // Second pass to get the most and least similar voters
        for (int i = 1; i < Data.voters; i++)
        {
            for (int j = i + 1; j <= Data.voters; j++)
            {
                diff = ArrayDifferences(voterScores[i.ToString()], voterScores[j.ToString()]);
                if (diff == minDiff)
                {
                    similarText.text += $"({i}, {j}), ";
                }
                if (diff == maxDiff)
                {
                    differentText.text += $"({i}, {j}), ";
                }
            }
        }
        similarText.text = similarText.text.TrimEnd(' ', ',');
        differentText.text = differentText.text.TrimEnd(' ', ',');
    }
}

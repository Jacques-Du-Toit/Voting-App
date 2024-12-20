using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    float[] InitialiseArray(int size, float value)
    {
        float[] array = new float[size];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = value;
        }
        return array;
    }

    void ColorTable(GameObject table)
    {
        Transform tablePos = table.transform;
        Transform row;
        Cell cell;
        float cellVal;
        Image cellCol;
        // Stores the best and worst values per column index
        float[] bestValues = InitialiseArray(4, -5f);
        float[] worstValues = InitialiseArray(4, 5f);

        Color softGreen = new Color(0.3f, 0.8f, 0.3f);
        Color softRed = new Color(0.8f, 0.3f, 0.3f);


        // First pass to get the best and worst values
        for (int row_i = 1; row_i < tablePos.childCount; row_i++) // skip first row as header
        {
            row = tablePos.GetChild(row_i);
            for (int col_i = 1; col_i < row.childCount; col_i++) // skip first col as index
            {
                cell = row.GetChild(col_i).GetComponent<Cell>();
                cellVal = float.Parse(cell.cellValue.text);
                if (cellVal > bestValues[col_i - 1])
                {
                    bestValues[col_i - 1] = cellVal;
                }
                if (cellVal < worstValues[col_i - 1])
                {
                    worstValues[col_i - 1] = cellVal;
                }
            }
        }

        // Second pass to change colors of the best and worst values
        for (int row_i = 1; row_i < tablePos.childCount; row_i++)
        {
            row = tablePos.GetChild(row_i);
            for (int col_i = 1; col_i < row.childCount; col_i++)
            {
                cell = row.GetChild(col_i).GetComponent<Cell>();
                cellVal = float.Parse(cell.cellValue.text);
                cellCol = cell.cellColor;
                if (cellVal == bestValues[col_i - 1])
                {
                    cellCol.color = softGreen;
                }
                else if (cellVal == worstValues[col_i - 1])
                {
                    cellCol.color = softRed;
                }
            }
        }
    }

    void PerChoiceResults(Dictionary<string, int[]> scores)
    {
        GameObject choiceStats;
        GameObject rowStats;
        float avg;
        float sumOfSquares;
        float std;
        float min;
        float max;

        // Give a title and mapping
        resultText.text += "<size=72>Per Option Results:</size>\n\n"; // Font size 72
        for (int i = 0; i < Data.choices.Count; i++)
        {
            resultText.text += $"<size=50>{i}: {Data.choices[i]}, </size>";
        }
        resultText.text = resultText.text.TrimEnd(',', ' ');

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
        ColorTable(choiceStats);
    }
}

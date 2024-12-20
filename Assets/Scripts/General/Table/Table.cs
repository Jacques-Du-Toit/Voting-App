using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    float[] InitialiseArray(int size, float value)
    {
        float[] array = new float[size];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = value;
        }
        return array;
    }

    public void ColorTable()
    {
        Transform tablePos = this.transform;
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
}

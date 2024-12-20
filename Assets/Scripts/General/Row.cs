using UnityEngine;

public class Row : MonoBehaviour
{    
    public void AddValues(params float[] values)
    {
        Transform thisPos = this.transform;
        Debug.Log($"Child count of {thisPos.name}");
        Debug.Log($"{thisPos.childCount}");

        foreach (var value in values)
        {
            print(value);
        }

        print(thisPos.childCount);

        for (int i = 0; i < thisPos.childCount; i++)
        {
            Transform childPos = thisPos.GetChild(i);
            childPos.GetComponent<Cell>().cellValue.text = values[i].ToString();
        }
    }
}

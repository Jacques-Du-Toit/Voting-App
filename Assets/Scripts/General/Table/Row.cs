using UnityEngine;

public class Row : MonoBehaviour
{    
    public void AddValues(params string[] values)
    {
        Transform thisPos = this.transform;

        for (int i = 0; i < thisPos.childCount; i++)
        {
            Transform childPos = thisPos.GetChild(i);
            childPos.GetComponent<Cell>().cellValue.text = values[i];
        }
    }
}

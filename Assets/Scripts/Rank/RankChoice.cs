using TMPro;
using UnityEngine;

public class RankChoice : MonoBehaviour
{
    public TMP_Text choiceText;
    RankSystem rankSystem;

    private void Start()
    {
        rankSystem = GetComponentInParent<RankSystem>();
    }

    public void MoveChoice(int direction)
    {
        rankSystem.MoveChoice(choiceText.text, direction);
    }
}
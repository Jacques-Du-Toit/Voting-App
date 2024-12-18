using TMPro;
using UnityEngine;

public class ChoiceParent : MonoBehaviour
{
    public TMP_Text choiceText;
    public TMP_InputField scoreInput;
    public int score;

    private void Start()
    {
        score = 42;
    }
}

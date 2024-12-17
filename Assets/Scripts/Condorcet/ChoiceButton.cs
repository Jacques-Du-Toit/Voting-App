using UnityEngine;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    public TMP_Text choiceText;
    public void AddText(string choice)
    {
        choiceText.text = choice;
    }

    CondorcetSystem condorcetScript;
    private void Start()
    {
        condorcetScript = GameObject.FindAnyObjectByType<CondorcetSystem>();
    }

    public void RemoveChoice()
    {
        condorcetScript.RemoveChoice(choiceText.text);
        Destroy(gameObject);
    }
}

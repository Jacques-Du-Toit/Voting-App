using UnityEngine;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] TMP_Text choiceText;
    UI UIScript; 

    private void Start()
    {
        UIScript = GameObject.FindAnyObjectByType<UI>();
    }

    public void AddText(string choice)
    {
        choiceText.text = choice;
    }

    public void RemoveChoice()
    {
        UIScript.RemoveChoice(choiceText.text);
        Destroy(gameObject);
    }
}

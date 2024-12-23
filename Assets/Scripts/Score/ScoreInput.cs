using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreInput : MonoBehaviour
{
    [SerializeField] GameObject parentChoice;
    [SerializeField] TMP_InputField thisField;
    [SerializeField] TMP_Text choiceText;

    ScoreSystem scoreScript;

    ColorBlock nullColors;

    private void Start()
    {
        nullColors = thisField.colors;
        scoreScript = transform.GetComponentInParent<ScoreSystem>();
    }

    public void SendInput()
    {
        if (thisField.text == "") {
            thisField.colors = nullColors;
            parentChoice.GetComponent<ChoiceParent>().score = 42;
            return;
        }
        scoreScript.ReceiveScore(choiceText.text, thisField.text);
    }

    public void SendClicked()
    {
        parentChoice.transform.SetAsFirstSibling();
    }
}



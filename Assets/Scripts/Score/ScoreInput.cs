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
        scoreScript = GameObject.Find("Score System").GetComponent<ScoreSystem>();
    }

    public void SendInput()
    {
        if (thisField.text == "") {
            thisField.colors = nullColors;
            return;
        }
        scoreScript.ReceiveScore(choiceText.text, thisField.text);
    }

    public void SendClicked()
    {
        parentChoice.transform.SetAsFirstSibling();
    }
}



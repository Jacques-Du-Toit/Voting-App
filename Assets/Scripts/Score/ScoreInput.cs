using UnityEngine;
using TMPro;

public class ScoreInput : MonoBehaviour
{
    [SerializeField] GameObject parentChoice;
    [SerializeField] TMP_InputField thisField;
    [SerializeField] TMP_Text choiceText;

    ScoreSystem scoreScript;

    private void Start()
    {
        scoreScript = GameObject.Find("Score System").GetComponent<ScoreSystem>();
    }

    public void SendInput()
    {
        scoreScript.ReceiveScore(thisField.text, choiceText.text);
    }

    public void SendClicked()
    {
        scoreScript.ChangeOrder(parentChoice, 0);
    }
}



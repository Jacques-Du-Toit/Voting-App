using UnityEngine;
using TMPro;

public class InputFieldGrabber : MonoBehaviour
{
    TMP_InputField thisField;
    string input;

    // The object that should react to the input
    [SerializeField] GameObject reactionGroup;

    private void Start()
    {
        thisField = GetComponent<TMP_InputField>();
    }

    public void GrabFromInputField()
    {
        input = thisField.text;
        thisField.text = "";
        thisField.ActivateInputField();
    }
}

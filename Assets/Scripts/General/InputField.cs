using UnityEngine;
using TMPro;

public class InputField : MonoBehaviour
{
    [SerializeField] TMP_InputField thisField;

    [SerializeField] GameObject reactionObject;
    IInputReceiver receiver;

    private void Start()
    {
        if (reactionObject == null) {
            Debug.Log("No reciever object for input.");
        }

        receiver = reactionObject.GetComponent<IInputReceiver>();

        if (receiver != null) {
            Debug.Log("Object does not implement IInputReceiver.");
        }
    }

    public void GrabFromInputField()
    {
        receiver.ReceiveInput(thisField.text, "input field type");
        thisField.text = "";
        thisField.ActivateInputField();
    }
}


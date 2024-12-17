using UnityEngine;
using TMPro;

public class InputField : MonoBehaviour
{
    [SerializeField] TMP_InputField thisField;

    [SerializeField] GameObject receiverObject;
    IInputReceiver receiver;

    private void Start()
    {
        if (receiverObject == null) {
            Debug.Log("No reciever object for input.");
        }

        receiver = receiverObject.GetComponent<IInputReceiver>();

        if (receiver == null) {
            Debug.Log("Object does not implement IInputReceiver.");
        }
    }

    public void SendInput()
    {
        receiver.ReceiveInput(thisField.text, "input field type");
        thisField.text = "";
        thisField.ActivateInputField();
    }
}


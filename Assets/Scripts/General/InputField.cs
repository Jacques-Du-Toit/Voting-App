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
            Debug.LogError("No reciever object for input.");
        }

        receiver = receiverObject.GetComponent<IInputReceiver>();

        if (receiver == null) {
            Debug.LogError("Object does not implement IInputReceiver.");
        }
    }

    public void SendInput()
    {
        receiver.ReceiveInput(thisField.text, gameObject);
        thisField.text = "";
        thisField.ActivateInputField();
    }
}


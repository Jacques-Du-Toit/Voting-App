using UnityEngine;
using TMPro;

public class InputFieldGrabber : MonoBehaviour
{
    private string inputText;

    // The object that should react to the input
    [SerializeField] private GameObject reactionGroup;

    public void GrabFromInputField(string input)
    {
        inputText = input;
        print(inputText);
    }
}

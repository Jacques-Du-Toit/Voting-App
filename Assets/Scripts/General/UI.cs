using UnityEngine;
public class UI : MonoBehaviour, IInputReceiver
{
    public GameObject canvas;
    public GameObject CondorcetSystem;

    public void CondorecetSystem()
    {
        Instantiate(CondorcetSystem, canvas.transform);
    }

    public void ReceiveInput(string inputText, GameObject inputField)
    {
        print(inputText);
        print(inputField.name);
        inputField.SetActive(false);
    }
}

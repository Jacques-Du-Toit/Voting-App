using UnityEngine;
public class UI : MonoBehaviour, IInputReceiver
{
    public GameObject canvas;
    public GameObject CondorcetSystem;

    public void CondorecetSystem()
    {
        Instantiate(CondorcetSystem, canvas.transform);
    }

    public void ReceiveInput(string inputText, string inputType)
    {
        print(inputText);
        print(inputType);
    }
}

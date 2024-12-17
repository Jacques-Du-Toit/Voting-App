using UnityEngine;
public class UI : MonoBehaviour
{
    public GameObject canvas;
    public GameObject CondorcetSystem;

    public void CondorecetSystem()
    {
        Instantiate(CondorcetSystem, canvas.transform);
    }
}

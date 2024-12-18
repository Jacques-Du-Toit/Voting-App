using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [SerializeField] GameObject scoreSystem;
    GameObject[] systems;

    int voters;

    int currentSystem = 0;

    void Start()
    {
        voters = Data.voters;
        voters = 3;
        systems = new GameObject[voters];
        InitialiseSystems();
        SetCurrentSystem(currentSystem);
    }

    void InitialiseSystems()
    {
        for (int i = 0; i < voters; i++)
        {
            systems[i] = Instantiate(scoreSystem, canvas.transform);
        }
    }

    void SetCurrentSystem(int i)
    {
        for (int j = 0; j < voters; j++)
        {
            systems[j].SetActive(i == j);
        }
    }
}

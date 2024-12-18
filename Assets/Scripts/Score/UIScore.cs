using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScore : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text whichVoter;
    [SerializeField] TMP_Text prevText;
    [SerializeField] TMP_Text nextText;
    [SerializeField] GameObject results;

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
        // Update the voter text
        whichVoter.text = $"Voter {i + 1}";

        // Update "previous" button text
        prevText.text = (i == 0) ? "Options" : $"Voter {i}";

        // Update "next" button text
        nextText.text = (i == voters - 1) ? "Results" : $"Voter {i + 2}";

        for (int j = 0; j < voters; j++)
        {
            systems[j].SetActive(i == j);
        }
    }

    void ChangeSystem(int direction)
    {
        currentSystem += direction;

        if (currentSystem < 0)
        {
            SceneManager.LoadScene("Options");
        }
        SetCurrentSystem(currentSystem);
        if (currentSystem >= voters)
        {
            
        }
    }
}

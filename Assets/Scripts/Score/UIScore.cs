using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScore : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text whichVoter;
    [SerializeField] TMP_Text prevText;
    [SerializeField] TMP_Text nextText;
    [SerializeField] GameObject results;

    [SerializeField] GameObject scoreSystem;
    GameObject[] systems;

    int voters;
    List<string> choices;

    int currentSystem = 0;

    void Start()
    {
        voters = Data.voters;
        choices = Data.choices;

        voters = 3;
        choices = new List<string> { "movie", "show", "book", "game", "arm-wrestle", "looooooooong optioooooooon", "first 20 mins of tomorrow i" };

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
        // Update Title
        title.text = "Rank Choices -5 to 5";

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

    Dictionary<string, int[]> CountScores()
    {
        Dictionary<string, int[]> allScores = new Dictionary<string, int[]>();
        Dictionary<string, int> systemScores = new Dictionary<string, int>();
        int score;

        foreach (string choice in choices)
        {
            allScores[choice] = new int[voters];
        }

        for (int i = 0;i < voters; i++)
        {
            systemScores = systems[i].GetComponent<ScoreSystem>().choiceScores;
            foreach(var entry in systemScores)
            {
                score = entry.Value;
                // Check for dummt score
                if (score == 42)
                {
                    score = 0;
                }
                allScores[entry.Key][i] = entry.Value;
            }

        }
        return allScores;
    }

    public void ChangeSystem(int direction)
    {
        currentSystem += direction;

        if (currentSystem < 0 || currentSystem == voters + 1)
        {
            SceneManager.LoadScene("Options");
        }

        SetCurrentSystem(currentSystem);
        
        if (currentSystem >= voters)
        {
            whichVoter.text = "";
            nextText.text = "Options";
            title.text = "Results";
            results.GetComponent<ResultScore>().RunResults(CountScores());
            results.SetActive(true);
        }
        else
        {
            results.SetActive(false);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
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

    Dictionary<string, int[]> CountChoiceScores()
    {
        Dictionary<string, int[]> choiceScores = new Dictionary<string, int[]>();
        Dictionary<string, int> systemScores = new Dictionary<string, int>();
        int score;

        foreach (string choice in choices)
        {
            choiceScores[choice] = new int[voters];
        }

        for (int i = 0;i < voters; i++)
        {
            systemScores = systems[i].GetComponent<ScoreSystem>().choiceScores;
            foreach(var entry in systemScores)
            {
                score = entry.Value;
                // Check for dummy score
                if (score == 42)
                {
                    score = 0;
                }
                choiceScores[entry.Key][i] = entry.Value;
            }

        }
        return choiceScores;
    }

    Dictionary<string, int[]> CountVoterScores()
    {
        Dictionary<string, int[]> voterScores = new Dictionary<string, int[]>();
        Dictionary<string, int> systemScores = new Dictionary<string, int>();

        for(int v = 1; v <= voters; v++)
        {
            voterScores[v.ToString()] = new int[choices.Count];
        }

        for (int i = 0; i < voters; i++)
        {
            systemScores = systems[i].GetComponent<ScoreSystem>().choiceScores;
            voterScores[(i + 1).ToString()] = systemScores.Values.ToArray();
        }
        return voterScores;
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
            results.GetComponent<ResultScore>().RunResults(CountChoiceScores(), CountVoterScores());
            results.SetActive(true);
        }
        else
        {
            results.SetActive(false);
        }
    }
}

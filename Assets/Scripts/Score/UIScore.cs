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
        for (int j = 0; j < voters; j++)
        {
            systems[j].SetActive(i == j);
        }

        if (i >= voters)
        {
            // We are past all voters
            return;
        }

        // Update Title
        title.text = systems[i].GetComponent<IVotingSystem>().Title;

        // Update the voter text
        whichVoter.text = $"Voter {i + 1}";

        // Update "previous" button text
        prevText.text = (i == 0) ? "Options" : $"Voter {i}";

        // Update "next" button text
        nextText.text = (i == voters - 1) ? "Results" : $"Voter {i + 2}";
    }

    Dictionary<string, int[]> CountChoiceScores()
    {
        // Returns a dictionary of this form
        // { "choiceOne": { firstVoterScore, ..., lastVoterScore }, "choiceTwo": { firstVoterScore, ..., lastVoterScore } }
        // -> { "movie": { 3, -1, 0 }, "game": { 1, 0, 4 } }

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
        // Returns a dictionary of this form
        // { "voterOne": { firstChoiceScore, ..., lastChoiceScore }, "voterTwo": { firstChoiceScore, ..., lastChoiceScore } }
        // -> { "1": { 3, 1 }, "2": { -1, 0 }, "3": { 0, 4 } }
        // (In a sense a transpose of CountChoiceScores)

        Dictionary<string, int[]> voterScores = new Dictionary<string, int[]>();
        Dictionary<string, int> systemScores = new Dictionary<string, int>();

        for(int v = 0; v < voters; v++)
        {
            voterScores[(v + 1).ToString()] = new int[choices.Count];
        }

        for (int v = 0; v < voters; v++)
        {
            systemScores = systems[v].GetComponent<ScoreSystem>().choiceScores;
            voterScores[(v + 1).ToString()] = systemScores.Values.ToArray();
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

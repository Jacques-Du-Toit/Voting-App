using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISystems : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text whichVoter;
    [SerializeField] TMP_Text prevText;
    [SerializeField] TMP_Text nextText;
    [SerializeField] GameObject results;

    [SerializeField] GameObject voteSystem;
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
            systems[i] = Instantiate(voteSystem, canvas.transform);
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
        title.text = systems[i].GetComponent<IVoteSystem>().Title;

        // Update the voter text
        whichVoter.text = $"Voter {i + 1}";

        // Update "previous" button text
        prevText.text = (i == 0) ? "Options" : $"Voter {i}";

        // Update "next" button text
        nextText.text = (i == voters - 1) ? "Results" : $"Voter {i + 2}";
    }

    Dictionary<string, int[]> CountChoiceVotes()
    {
        Dictionary<string, int[]> choiceVotes = new Dictionary<string, int[]>();
        Dictionary<string, int> systemVotes = new Dictionary<string, int>();
        int vote;

        foreach (string choice in choices)
        {
            choiceVotes[choice] = new int[voters];
        }

        for (int i = 0;i < voters; i++)
        {
            systemVotes = systems[i].GetComponent<IVoteSystem>().ChoiceVotes;
            foreach(var entry in systemVotes)
            {
                vote = entry.Value;
                // Check for dummy vote
                if (vote == 42)
                {
                    vote = 0;
                }
                choiceVotes[entry.Key][i] = entry.Value;
            }

        }
        return choiceVotes;
    }

    Dictionary<string, int[]> CountVoterVotes()
    {
        Dictionary<string, int[]> voterVotes = new Dictionary<string, int[]>();
        Dictionary<string, int> systemVotes = new Dictionary<string, int>();

        for(int v = 1; v <= voters; v++)
        {
            voterVotes[v.ToString()] = new int[choices.Count];
        }

        for (int i = 0; i < voters; i++)
        {
            systemVotes = systems[i].GetComponent<IVoteSystem>().ChoiceVotes;
            voterVotes[(i + 1).ToString()] = systemVotes.Values.ToArray();
        }
        return voterVotes;
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
            results.GetComponent<Results>().RunResults(CountChoiceVotes(), CountVoterVotes());
            results.SetActive(true);
        }
        else
        {
            results.SetActive(false);
        }
    }
}

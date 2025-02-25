using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISystems : MonoBehaviour
{
    // A script used to generate a 'votingSystem' per voter
    // for example, a score system per voter (multiple choices laid out that they need to score).
    // The point of this being that a generic votingSystem can be passed (score, ranked etc.)

    [SerializeField] GameObject canvas; // ui container
    [SerializeField] TMP_Text title; // displayed at top as a guide on how to vote (SHOULD INHERIT FROM VOTING SYSTEM CHOSEN)
    [SerializeField] TMP_Text whichVoter; // displayed at the top to say who current voter is
    [SerializeField] TMP_Text prevText; // text on the button at top left
    [SerializeField] TMP_Text nextText; // text on button at top right
    [SerializeField] GameObject results; // gameobject that displays all the results of the votes on choices

    [SerializeField] GameObject votingSystem; // the voting system being used
    GameObject[] systems; // contains an instantiated copy of the votingSystem for each voter

    int voters;
    List<string> choices;

    int currentSystem = 0; // which voters system we are currently on

    void Start()
    {
        // Load data
        voters = Data.voters;
        choices = Data.choices;
        // Set up the systems and set to the first voters system
        systems = new GameObject[voters];
        InitialiseSystems();
        SetCurrentSystem(currentSystem);
    }

    void InitialiseSystems()
    {
        // Instanitiates a copy of the votingSystem per voter
        for (int i = 0; i < voters; i++)
        {
            systems[i] = Instantiate(votingSystem, canvas.transform);
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

    List<string> GetSystemNames()
    {
        // Returns the Name of each system used
        // (in future want to be able to use different systems per voter)
        List<string> systemNames = new List<string>();
        foreach (GameObject system in systems)
        {
            systemNames.Add(system.GetComponent<IVotingSystem>().Name);
        }
        return systemNames;
    }

    List<Dictionary<string, int>> GetVoterVotes()
    {
        // Returns the votes of each voter in order as a list of dictionaries
        // i.e. { { "choice1": 1, "choice2": .. }, { "choice1": -3, "choice2": .. } }
        List<Dictionary<string, int>> voterVotes = new List<Dictionary<string, int>>();

        foreach (GameObject system in systems)
        {
            voterVotes.Add(system.GetComponent<IVotingSystem>().ChoiceValues);
        }
        return voterVotes;
    }

    public void ChangeSystem(int direction)
    {
        // Moves the system to either the next voter or previous voter
        //  if at first voter - moves back to the options menu
        //  if at last voter - displays results

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
            results.GetComponent<ResultScore>().RunResults(
                GetSystemNames(), GetVoterVotes()
                );
            results.SetActive(true);
        }
        else
        {
            results.SetActive(false);
        }
    }
}

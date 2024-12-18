using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConVoterScript : MonoBehaviour
{
    CondorcetSystem condorcetScript;
    public TMP_Text voterText;
    private void Start()
    {
        condorcetScript = GameObject.FindAnyObjectByType<CondorcetSystem>();
        voterText.text = $"Voter {condorcetScript.voter}";
        StartCoroutine(ShowVoter());
    }

    IEnumerator<WaitForSeconds> ShowVoter()
    {
        yield return new WaitForSeconds(1f);
        condorcetScript.roundIndex = 0;
        condorcetScript.NextRound();
        Destroy(gameObject);
    }
}

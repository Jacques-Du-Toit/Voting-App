using UnityEngine;
using System.Collections.Generic;

public interface IVoteSystem
{
    string Title { get; }
    Dictionary<string, int> ChoiceVotes { get; }
}

using UnityEngine;
using System.Collections.Generic;

public interface IVotingSystem
{
    // A voting system will be a specific instantiation of a prefab for a specific voter.
    // The voting system should:
    // - record the name of the system
    // - record the voters votes (will look different for different voting systems)
    //      - i.e. for Score - each voting system will return a set of scores for each choice based on it's voter
    //      - will need to go to that voting system to interpret its values

    string Name { get; }
    Dictionary<string, int[]> ChoiceValues {  get; }
}

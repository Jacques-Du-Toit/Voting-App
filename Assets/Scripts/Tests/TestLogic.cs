using NUnit.Framework;
using System.Collections.Generic;

[TestFixture]
public class TestLogic
{
    private List<string> AllocateChoices(List<List<string>> voterChoiceRankings)
    {
        /// 
        /// <summary>
        /// Allocates choices to voters in a way that:
        /// 1. Minimizes the maximum distance between a voter's top choice and the choice they receive.
        ///    - Example: If voter A gets their 2nd choice and voter B gets their 3rd choice, the max distance is 3.
        ///    - The goal is fairness: it is preferable for all voters to receive their 4th choice 
        ///      rather than some getting their 1st while another gets their 6th.
        /// 2. Among allocations with the minimized maximum distance, minimize the average distance.
        ///    - Example: If voter A and B get their 1st choice while voter C gets their 2nd,
        ///      this is preferred over all three getting their 2nd choice.
        /// 3. If multiple allocations meet the above criteria, prioritize the first valid allocation found.
        /// </summary>
        /// <param name="voterChoiceRankings">
        /// A list of ranked choices for each voter.
        /// Each inner list represents a voter's ranked preferences, with the first element being their top choice.
        /// Example: { { "2", "1", "3"}, { "3", "2", "1" } } means:
        /// - Voter A prefers "2" > "1" > "3"
        /// - Voter B prefers "3" > "2" > "1"
        /// </param>
        /// <returns>
        /// A list where each element corresponds to the allocated choice for the voter at that index.
        /// Example: { "2", "3" } means:
        /// - Voter A receives "2"
        /// - Voter B receives "3"
        /// </returns>
        /// <example>
        /// <code>
        /// List<List<string>> voterChoices = new List<List<string>>
        /// {
        ///     new List<string> { "2", "1", "3" },
        ///     new List<string> { "3", "2", "1" }
        /// };
        /// 
        /// List<string> choiceAllocations = AllocateChoices(voterChoices);
        /// // choiceAllocations == { "2", "3" }
        /// </code>
        /// </example>
        /// 
        return new List<string> { "2", "3", "1" };
    }

    [Test]
    public void TestAllocationLogic()
    {
        List<List<string>> voterChoices = new List<List<string>>
        {
             new List<string> { "2", "1", "3" },
             new List<string> { "3", "2", "1" }
        };
        
        List<string> choiceAllocations = AllocateChoices(voterChoices);
        CollectionAssert.AreEqual(choiceAllocations, new List<string> { "2", "3" });
    }
}



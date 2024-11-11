namespace Pumpkin.Beer.Taste.ViewModels.Scores;

using System.Collections.Generic;
using Pumpkin.Beer.Taste.ViewModels.Scores;

public class IndexViewModel
{
    public decimal TotalScore { get; set; }

    public decimal AverageScore { get; set; }

    public decimal AmountOfVotes { get; set; }

    public IndexBlindItemViewModel BlindItem { get; set; } = null!;

    public List<IndexVoteViewModel> Votes { get; set; } = [];
}

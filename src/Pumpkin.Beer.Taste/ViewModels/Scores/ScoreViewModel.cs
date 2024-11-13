namespace Pumpkin.Beer.Taste.ViewModels.Scores;

using System.Collections.Generic;
using Pumpkin.Beer.Taste.ViewModels.Scores;

public class ScoreViewModel
{
    public int TotalScore { get; set; }

    public int AverageScore { get; set; }

    public double AmountOfVotes { get; set; }

    public ScoreBlindItemViewModel BlindItem { get; set; } = null!;
}

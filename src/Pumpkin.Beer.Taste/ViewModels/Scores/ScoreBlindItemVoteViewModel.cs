namespace Pumpkin.Beer.Taste.ViewModels.Scores;

using System.Collections.Generic;
using Pumpkin.Beer.Taste.ViewModels.Scores;

public class ScoreBlindItemVoteViewModel
{
    public int Score { get; set; }

    public string? Note { get; set; }

    public bool Public { get; set; }

    public string CreatedByUserDisplayName { get; set; } = null!;
}

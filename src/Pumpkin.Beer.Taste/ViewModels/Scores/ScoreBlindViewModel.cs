namespace Pumpkin.Beer.Taste.ViewModels.Scores;

using Pumpkin.Beer.Taste.Data;

public class ScoreBlindViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTimeOffset? Started { get; set; }

    public DateTimeOffset? Closed { get; set; }

    public List<ScoreViewModel> BlindItemScores { get; set; } = [];
}

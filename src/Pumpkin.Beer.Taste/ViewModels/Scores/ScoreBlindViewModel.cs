namespace Pumpkin.Beer.Taste.ViewModels.Scores;

using Pumpkin.Beer.Taste.Data;

public class ScoreBlindViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTime Started { get; set; }

    public DateTime Closed { get; set; }

    public List<ScoreViewModel> BlindItemScores { get; set; } = [];
}

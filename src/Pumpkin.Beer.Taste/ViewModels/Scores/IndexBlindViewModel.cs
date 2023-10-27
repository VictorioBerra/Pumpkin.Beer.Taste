namespace Pumpkin.Beer.Taste.ViewModels.Scores;

using Pumpkin.Beer.Taste.Data;

public class IndexBlindViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTime? Started { get; set; }

    public DateTime? Closed { get; set; }
}

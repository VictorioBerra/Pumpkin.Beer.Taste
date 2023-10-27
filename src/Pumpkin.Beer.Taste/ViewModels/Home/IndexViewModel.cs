namespace Pumpkin.Beer.Taste.ViewModels.Home;

using Pumpkin.Beer.Taste.ViewModels.ManageBlind;

public class IndexViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTime? Started { get; set; }

    public DateTime? Closed { get; set; }
}

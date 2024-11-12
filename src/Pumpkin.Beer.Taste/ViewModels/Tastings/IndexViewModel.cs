namespace Pumpkin.Beer.Taste.ViewModels.Tastings;

public class IndexViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public int NumMembers { get; set; }

    public DateTimeOffset? Started { get; set; }

    public DateTimeOffset? Closed { get; set; }

    public string CreatedByUserDisplayName { get; set; } = null!;
}

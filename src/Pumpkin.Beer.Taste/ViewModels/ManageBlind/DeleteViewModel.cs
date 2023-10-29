namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

public class DeleteViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public string CreatedByUserDisplayName { get; set; } = null!;

    public DateTimeOffset? Started { get; set; }

    public DateTimeOffset? Closed { get; set; }
}

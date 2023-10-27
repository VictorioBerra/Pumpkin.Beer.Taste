namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

public class DeleteViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public string CreatedByUserDisplayName { get; set; } = null!;

    public DateTime? Started { get; set; }

    public DateTime? Closed { get; set; }
}

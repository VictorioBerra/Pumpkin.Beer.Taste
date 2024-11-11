namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

public class EditViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTimeOffset? Started { get; set; }

    public DateTimeOffset? Closed { get; set; }

    public List<EditItemViewModel> BlindItems { get; set; } = [];
}

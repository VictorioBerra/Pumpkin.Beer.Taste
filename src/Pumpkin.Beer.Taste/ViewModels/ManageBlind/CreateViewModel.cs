namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

using Pumpkin.Beer.Taste.Data;

public class CreateViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTimeOffset? Started { get; set; }

    public DateTimeOffset? Closed { get; set; }

    public List<CreateItemViewModel> BlindItems { get; set; } = [];
}

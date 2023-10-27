namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

using Pumpkin.Beer.Taste.Data;

public class CreateViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTime? Started { get; set; }

    public DateTime? Closed { get; set; }

    public List<CreateItemViewModel> BlindItems { get; set; } = new();
}

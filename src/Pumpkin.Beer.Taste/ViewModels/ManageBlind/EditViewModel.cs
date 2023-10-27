namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

public class EditViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTime? Started { get; set; }

    public DateTime? Closed { get; set; }

    public List<EditItemViewModel> BlindItems { get; set; } = new();
}

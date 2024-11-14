namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

using System.ComponentModel.DataAnnotations;
using Pumpkin.Beer.Taste.Data;

public class CreateViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTimeOffset? Started { get; set; }

    public DateTimeOffset? Closed { get; set; }

    [MinLength(1, ErrorMessage = "At least one item is required.")]
    [MaxLength(26, ErrorMessage = "No more than 26 items are allowed.")]
    public List<CreateItemViewModel> BlindItems { get; set; } = [];
}

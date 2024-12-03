namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Pumpkin.Beer.Taste.Data;

public class CreateViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTime? Started { get; set; }

    public DateTime? Closed { get; set; }

    [DisplayName("Event Start/Close Time Zone")]
    public string StartedAndClosedIANATimeZoneId { get; set; } = null!;

    [MinLength(1, ErrorMessage = "At least one item is required.")]
    [MaxLength(26, ErrorMessage = "No more than 26 items are allowed.")]
    public List<CreateItemViewModel> BlindItems { get; set; } = [];
}

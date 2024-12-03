namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

using System.ComponentModel;

public class EditViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasVotes { get; set; }

    public DateTime Started { get; set; }

    public DateTime Closed { get; set; }

    [DisplayName("Event Start/Close Time Zone")]
    public string StartedAndClosedIANATimeZoneId { get; set; } = null!;
}

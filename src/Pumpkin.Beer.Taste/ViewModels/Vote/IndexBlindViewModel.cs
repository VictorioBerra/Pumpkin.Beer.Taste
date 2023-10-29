namespace Pumpkin.Beer.Taste.ViewModels.Vote;

using System.ComponentModel.DataAnnotations;

public class IndexBlindViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTimeOffset? Started { get; set; }

    public DateTimeOffset? Closed { get; set; }
}

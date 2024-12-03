namespace Pumpkin.Beer.Taste.ViewModels.Vote;

using System.ComponentModel.DataAnnotations;

public class IndexBlindViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Started { get; set; }

    public DateTime Closed { get; set; }
}

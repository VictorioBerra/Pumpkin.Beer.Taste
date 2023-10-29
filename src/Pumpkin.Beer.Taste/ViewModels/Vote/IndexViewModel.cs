namespace Pumpkin.Beer.Taste.ViewModels.Vote;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class IndexViewModel
{
    [Range(1, 5)]
    public int Score { get; set; }

    public int BlindItemId { get; set; }

    public string? Note { get; set; }

    public bool Public { get; set; }
}

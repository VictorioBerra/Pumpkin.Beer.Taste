namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

using System.ComponentModel.DataAnnotations;
using Pumpkin.Beer.Taste.Data;

public class CreateItemViewModel
{
    [Required(ErrorMessage = "Item missing name.")]
    public string Name { get; set; } = null!;

    public int Ordinal { get; set; }
}

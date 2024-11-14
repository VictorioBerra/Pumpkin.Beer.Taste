namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

using System.ComponentModel.DataAnnotations;

public class EditItemViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Item missing name.")]
    public string Name { get; set; } = null!;

    public int Ordinal { get; set; }
}

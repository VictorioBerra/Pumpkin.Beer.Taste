namespace Pumpkin.Beer.Taste.ViewModels.Vote;

using System.ComponentModel.DataAnnotations;

public class IndexBlindItemViewModel
{
    public int Id { get; set; }

    public int Ordinal { get; set; }

    public string Letter => EnAlphaExtensions.IndexToColumn(this.Ordinal);
}

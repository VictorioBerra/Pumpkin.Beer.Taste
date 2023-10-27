namespace Pumpkin.Beer.Taste.ViewModels.Scores;

using System.Collections.Generic;
using Pumpkin.Beer.Taste.ViewModels.Scores;

public class IndexBlindItemViewModel
{
    public string Name { get; set; } = null!;

    public int Ordinal { get; set; }

    public string Letter => EnAlphaExtensions.IndexToColumn(this.Ordinal);
}

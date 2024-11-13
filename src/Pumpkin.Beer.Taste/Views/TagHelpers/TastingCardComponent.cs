namespace Pumpkin.Beer.Taste.Views.TagHelpers;

using Microsoft.AspNetCore.Razor.TagHelpers;
using TechGems.RazorComponentTagHelpers;

[HtmlTargetElement("tasting-card")]
public class TastingCardComponent : RazorComponentTagHelper
{
    public TastingCardComponent()
        : base("~/Views/TagHelpers/TastingCardComponent.cshtml")
    {
    }
}

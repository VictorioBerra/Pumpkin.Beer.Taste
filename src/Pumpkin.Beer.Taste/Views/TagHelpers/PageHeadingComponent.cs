namespace Pumpkin.Beer.Taste.Views.TagHelpers;

using Microsoft.AspNetCore.Razor.TagHelpers;
using TechGems.RazorComponentTagHelpers;

// https://razor-components.techgems.net/docs/basic-usage/
[HtmlTargetElement("page-heading")]
public class PageHeadingComponent : RazorComponentTagHelper
{
    public PageHeadingComponent()
        : base("~/Views/TagHelpers/PageHeadingComponent.cshtml")
    {
    }

    [HtmlAttributeName("heading")]
    public string Heading { get; set; } = string.Empty;
}

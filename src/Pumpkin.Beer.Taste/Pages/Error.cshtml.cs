namespace Pumpkin.Beer.Taste.Pages;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class ErrorModel(ILogger<ErrorModel> logger) : PageModel
{
    public string RequestId { get; set; } = null!;

    public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

    public void OnGet()
    {
        this.RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier;
        logger.LogError("User hit error page {RequestId} {Name}", this.RequestId, this.User.Identity?.Name);
    }
}

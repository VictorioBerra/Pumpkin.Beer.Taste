namespace Pumpkin.Beer.Taste.Pages;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Eblindtensions;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.Services;
using Pumpkin.Beer.Taste.ViewModels.Home;
using SharpRepository.Repository;
using SharpRepository.Repository.Specifications;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class IndexModel() : PageModel
{
    public IActionResult OnGet()
    {
        if (!this.User.Identity?.IsAuthenticated ?? false)
        {
            return this.Page();
        }

        return this.RedirectToPage("/Home");
    }
}

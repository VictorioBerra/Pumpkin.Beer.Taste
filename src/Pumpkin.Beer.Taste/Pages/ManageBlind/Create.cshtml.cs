namespace Pumpkin.Beer.Taste.Pages.BlindPages;

using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NanoidDotNet;
using Pumpkin.Beer.Taste.Data;
using Pumpkin.Beer.Taste.Extensions;
using Pumpkin.Beer.Taste.ViewModels.ManageBlind;
using SharpRepository.Repository;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;

[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Razor pages.")]
public class CreateModel(
    IRepository<Blind, int> blindRepository) : PageModel
{
    [BindProperty]
    public CreateViewModel Blind { get; set; } = null!;

    [BindProperty]
    public IFormFile? Upload { get; set; }

    public IActionResult OnGet() => this.Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!this.ModelState.IsValid)
        {
            return this.Page();
        }

        if (this.Blind.Started is null || this.Blind.Closed is null)
        {
            this.ModelState.AddPageError("Start and End dates are required.");
            return this.Page();
        }

        byte[]? coverPhoto = null;
        if (this.Upload is not null)
        {
            var allowedExtensions = new[] { ".webp", ".png", ".jpg", ".jpeg" };
            var extension = Path.GetExtension(this.Upload.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                this.ModelState.AddPageError("Invalid file type. Only webp, png, jpg, and gif are allowed.");
                return this.Page();
            }

            using var memoryStream = new MemoryStream();
            this.Upload.CopyTo(memoryStream);
            memoryStream.Position = 0;

            using var image = Image.Load(memoryStream);

            if (image.Width > 400 || image.Height > 400)
            {
                this.ModelState.AddPageError("Image dimensions should not exceed 400x400 pixels.");
                return this.Page();
            }

            // Strip EXIF data
            image.Metadata.ExifProfile = null;

            using var outputMemoryStream = new MemoryStream();
            await image.SaveAsJpegAsync(outputMemoryStream);
            coverPhoto = outputMemoryStream.ToArray();
        }

        var blind = new Blind
        {
            InviteCode = await Nanoid.GenerateAsync(alphabet: "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", size: 4),
            Name = this.Blind.Name,

            CoverPhoto = coverPhoto,

            // Strip offset
            Started = new DateTimeOffset(this.Blind.Started.Value.DateTime, TimeSpan.Zero),
            Closed = new DateTimeOffset(this.Blind.Closed.Value.DateTime, TimeSpan.Zero),

            BlindItems = this.Blind.BlindItems.Select((x, i) => new BlindItem
            {
                Name = x.Name,
                Ordinal = i,
            }).ToList(),

            // Test this and make sure creator gets "invited"
            // An accepted invite is essentially the BlindId + CreatedByUserId (should be set automatically)
            UserInvites = [
                new UserInvite
                {
                }
            ],
        };

        blindRepository.Add(blind);

        return this.RedirectToPage("./Index");
    }
}

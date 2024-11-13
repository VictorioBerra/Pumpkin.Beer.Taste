namespace Pumpkin.Beer.Taste.ViewModels.Home;

public class IndexViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? CoverPhotoBase64 { get; set; }

    public bool HasVotes { get; set; }

    public int NumMembers { get; set; }

    public DateTimeOffset? Started { get; set; }

    public DateTimeOffset? Closed { get; set; }

    public string CreatedByUserId { get; set; } = null!;

    public string CreatedByUserDisplayName { get; set; } = null!;
}

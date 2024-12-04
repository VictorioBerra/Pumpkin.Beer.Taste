namespace Pumpkin.Beer.Taste.ViewModels.Home;

public class IndexViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? CoverPhotoBase64 { get; set; }

    public bool HasVotes { get; set; }

    public int NumMembers { get; set; }

    public DateTime StartedUtc { get; set; }

    public DateTime ClosedUtc { get; set; }

    public DateTime Started { get; set; }

    public DateTime Closed { get; set; }

    // This is just for the projection
    public string StartsInWindowsTimeZoneId { get; set; } = null!;

    // Just using startedWindowsTimeZone as both for now, maybe one day we'll allow setting different time zones for start and close
    // Should work right now though if we did, just wanted to simplify UI.
    public string StartsAndClosesIANATimeZoneId { get; set; } = null!;

    public string CreatedByUserId { get; set; } = null!;

    public string CreatedByUserDisplayName { get; set; } = null!;
}

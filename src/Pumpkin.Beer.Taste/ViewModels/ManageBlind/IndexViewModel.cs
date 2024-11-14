namespace Pumpkin.Beer.Taste.ViewModels.ManageBlind;

public class IndexViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? CoverPhotoBase64 { get; set; }

    public string? JoinAndVoteLink { get; set; }

    public string? QRCodeBase64 { get; set; }

    public string InviteCode { get; set; } = null!;

    public int NumMembers { get; set; }

    public int NumItems { get; set; }

    public int NumVotes { get; set; }

    public bool IsOpen { get; set; }

    public DateTimeOffset? Started { get; set; }

    public DateTimeOffset? Closed { get; set; }

    public string CreatedByUserDisplayName { get; set; } = null!;
}

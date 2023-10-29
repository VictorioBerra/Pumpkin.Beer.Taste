namespace Pumpkin.Beer.Taste.Data;

public class BlindVote : AuditableEntity
{
    public int Id { get; set; }

    public int Score { get; set; }

    public bool Public { get; set; }

    public string? Note { get; set; }

    public int BlindItemId { get; set; }

    public BlindItem BlindItem { get; set; } = null!;
}

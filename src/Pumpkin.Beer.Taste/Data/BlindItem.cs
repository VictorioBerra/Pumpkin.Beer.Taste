namespace Pumpkin.Beer.Taste.Data;

using System.Collections.Generic;

public class BlindItem : AuditableEntity
{
    public BlindItem() => this.BlindVotes = new HashSet<BlindVote>();

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Ordinal { get; set; }

    public int BlindId { get; set; }

    public Blind Blind { get; set; } = null!;

    public ICollection<BlindVote> BlindVotes { get; set; }
}

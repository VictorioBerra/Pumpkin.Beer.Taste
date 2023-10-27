namespace Pumpkin.Beer.Taste.Data;

using System;
using System.Collections.Generic;

public class Blind : AuditableEntity
{
    public Blind() => this.BlindItems = new HashSet<BlindItem>();

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? Started { get; set; }

    public DateTime? Closed { get; set; }

    public ICollection<BlindItem> BlindItems { get; set; }
}

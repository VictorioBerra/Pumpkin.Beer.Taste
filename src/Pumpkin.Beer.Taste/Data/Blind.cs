namespace Pumpkin.Beer.Taste.Data;

using System;
using System.Collections.Generic;

public class Blind : AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string InviteCode { get; set; } = null!;

    public byte[]? CoverPhoto { get; set; }

    public DateTimeOffset? Started { get; set; }

    public DateTimeOffset? Closed { get; set; }

    public ICollection<UserInvite> UserInvites { get; set; } = [];

    public ICollection<BlindItem> BlindItems { get; set; } = [];
}

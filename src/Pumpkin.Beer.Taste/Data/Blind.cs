namespace Pumpkin.Beer.Taste.Data;

using System;
using System.Collections.Generic;

public class Blind : AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string InviteCode { get; set; } = null!;

    public byte[]? CoverPhoto { get; set; }

    public DateTime StartedUtc { get; set; }

    public DateTime ClosedUtc { get; set; }

    public string StartedWindowsTimeZoneId { get; set; } = null!;

    public string ClosedWindowsTimeZoneId { get; set; } = null!;

    public ICollection<UserInvite> UserInvites { get; set; } = [];

    public ICollection<BlindItem> BlindItems { get; set; } = [];
}

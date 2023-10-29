namespace Pumpkin.Beer.Taste.Data;

using System;
using System.ComponentModel.DataAnnotations;

public abstract class AuditableEntity
{
    public DateTimeOffset CreatedDate { get; set; }

    [Required]
    public string CreatedByUserId { get; set; } = null!;

    public DateTimeOffset UpdatedDate { get; set; }

    [Required]
    public string UpdatedByUserId { get; set; } = null!;

    public string CreatedByUserDisplayName { get; set; } = null!;

    public string UpdatedByUserDisplayName { get; set; } = null!;
}

namespace Pumpkin.Beer.Taste.Data;

using System;
using System.ComponentModel.DataAnnotations;

public abstract class AuditableEntity
{
    public DateTime CreatedDate { get; set; }

    [Required]
    public string CreatedByUserId { get; set; } = null!;

    public DateTime UpdatedDate { get; set; }

    [Required]
    public string UpdatedByUserId { get; set; } = null!;

    public string CreatedByUserDisplayName { get; set; } = null!;

    public string UpdatedByUserDisplayName { get; set; } = null!;
}

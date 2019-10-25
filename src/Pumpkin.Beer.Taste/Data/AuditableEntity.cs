using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Data
{
    public abstract class AuditableEntity
    {
        public DateTimeOffset? CreatedDate { get; set; }

        [Required]
        public string CreatedByUserId { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [Required]
        public string UpdatedByUserId { get; set; }

        public virtual IdentityUser CreatedByUser { get; set; }

        public virtual IdentityUser UpdatedByUser { get; set; }
    }
}

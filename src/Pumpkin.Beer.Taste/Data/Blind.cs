using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Data
{
    public class Blind : AuditableEntity
    {
        public Blind()
        {
            BlindItems = new HashSet<BlindItem>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? Started { get; set; }

        public DateTimeOffset? Closed { get; set; }

        public ICollection<BlindItem> BlindItems { get; set; }
    }
}

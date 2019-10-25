using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Data
{
    public class BlindVote : AuditableEntity
    {
        public int Id { get; set; }

        public int Score { get; set; }

        public int BlindItemId { get; set; }

        public BlindItem BlindItem { get; set; }

        public int BlindItemOrdinal { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}

using Pumpkin.Beer.Taste.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Models
{
    public class BlindDto
    {
        public BlindDto()
        {
            BlindItems = new List<BlindItemDto>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool HasVotes { get; set; }

        public string CreatedByUsername { get; set; }

        public DateTime? Started { get; set; }

        public DateTime? Closed { get; set; }

        public List<BlindItemDto> BlindItems { get; set; }
    }
}

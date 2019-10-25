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

        public string CreatedByUsername { get; set; }

        public DateTimeOffset? Started { get; set; }

        public DateTimeOffset? Closed { get; set; }

        public DateTimeOffset Created { get; set; }

        public List<BlindItemDto> BlindItems { get; set; }
    }
}

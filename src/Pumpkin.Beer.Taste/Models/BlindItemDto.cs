using Pumpkin.Beer.Taste.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Models
{
    public class BlindItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BlindId { get; set; }

        public BlindDto Blind { get; set; }

    }
}

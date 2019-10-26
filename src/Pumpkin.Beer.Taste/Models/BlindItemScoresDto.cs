using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Models
{
    public class BlindItemScoresDto
    {
        public BlindItemScoresDto()
        {
            Notes = new List<string>();
        }

        public BlindItemDto BlindItem { get; set; }

        public decimal TotalScore { get; set; }

        public decimal AmountOfVotes { get; set; }

        public List<string> Notes { get; set; }
    }
}

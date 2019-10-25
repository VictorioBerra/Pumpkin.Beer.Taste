using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Data
{
    public class Blind
    {
        public Blind()
        {
            BlindItems = new HashSet<BlindItem>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Started { get; set; }

        public DateTime? Closed { get; set; }

        public DateTime Created { get; set; }

        public ICollection<BlindItem> BlindItems { get; set; }
    }
}

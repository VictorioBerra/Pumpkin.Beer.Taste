using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Data
{
    public class BlindItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Make { get; set; }

        public int BlindId { get; set; }

        public Blind Blind { get; set; }

    }
}

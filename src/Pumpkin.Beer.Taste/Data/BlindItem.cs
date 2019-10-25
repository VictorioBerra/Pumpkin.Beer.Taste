using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Data
{
    public class BlindItem : AuditableEntity
    {
        public BlindItem()
        {
            BlindVotes = new HashSet<BlindVote>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int ordinal { get; set; }

        public int BlindId { get; set; }

        public Blind Blind { get; set; }

        public ICollection<BlindVote> BlindVotes { get; set; }

    }
}

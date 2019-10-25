using Pumpkin.Beer.Taste.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Models
{
    public class BlindItemDto
    {
        public int Id { get; set; }

        [Range(0, 50)]
        public int Ordinal { get; set; }

        public string Name { get; set; }

        public string Letter 
        { 
            get
            {
                return enAlphaExtensions.IndexToColumn(this.Ordinal);
            }
        }

        public int BlindId { get; set; }

        public BlindDto Blind { get; set; }

    }
}

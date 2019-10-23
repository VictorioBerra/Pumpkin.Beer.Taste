﻿using Microsoft.AspNetCore.Identity;
using Pumpkin.Beer.Taste.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pumpkin.Beer.Taste.Models
{
    public class BlindVoteDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public int BlindItemId { get; set; }

        public BlindItemDto BlindItem { get; set; }

        public int BlindItemOrdinal { get; set; }
    }
}
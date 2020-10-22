﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Space
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string Name { get; set; }
        public bool IsAccessible { get; set; }
        public DateTime? OpenFrom { get; set; } //nullable property
        public DateTime? OpenTo { get; set; } //nullable property
        public decimal DailyRate { get; set; }
        public int MaxOccupancy { get; set; }
    }
}

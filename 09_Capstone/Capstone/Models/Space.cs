using System;
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
        public string OpenFrom { get; set; } 
        public string OpenTo { get; set; } 
        public decimal DailyRate { get; set; }
        public int MaxOccupancy { get; set; }

        public override string ToString()
        {
            return Id.ToString().PadRight(4) + Name.PadRight(25) + IsAccessible.ToString().PadRight(20) + 
                OpenFrom.PadRight(10) +  OpenTo.PadRight(10) +
                DailyRate.ToString("C").PadRight(15) + MaxOccupancy.ToString().PadRight(10);
        }
    }
}

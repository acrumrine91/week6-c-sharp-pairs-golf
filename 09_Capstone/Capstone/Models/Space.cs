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
        public int? OpenFrom { get; set; } //nullable property
        public int? OpenTo { get; set; } //nullable property
        public decimal DailyRate { get; set; }
        public int MaxOccupancy { get; set; }

        public override string ToString()
        {
            return Id.ToString().PadRight(5) + Name.PadRight(20) + IsAccessible.ToString().PadRight(7) + 
                OpenFrom.ToString().PadRight(5) +  OpenTo.ToString().PadRight(5) +
                DailyRate.ToString("C").PadRight(15) + MaxOccupancy.ToString().PadRight(8);
        }
    }
}

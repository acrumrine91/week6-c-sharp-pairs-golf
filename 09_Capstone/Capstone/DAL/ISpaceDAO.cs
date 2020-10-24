using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISpaceDAO
    {
        IList<Space> GetVenueSpaces(int venueNum);

        Space GetBookedSpaceDetails(string chosenID);
    }
}

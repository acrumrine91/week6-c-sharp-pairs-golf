using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        IList<Reservation> GetReservations(IList<Space> spaces);
    }
}

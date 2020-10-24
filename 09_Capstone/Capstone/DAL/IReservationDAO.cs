using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        IList<Reservation> GetReservations(IList<Space> spaces);
        bool IsDateAvailable(IList<Reservation> reservations, Space space, DateTime startDate, int numOfDays);
        bool IsSpaceOperating(Space space, DateTime startDate, int numOfDays);
        bool IsBookingBelowMaxOcc(Space space, int peopleAttending);

        string AddReservationToSql(string spaceIDChosen, string reservedFor, DateTime startDate, int numOfDays, int peopleAttending);

    }
}

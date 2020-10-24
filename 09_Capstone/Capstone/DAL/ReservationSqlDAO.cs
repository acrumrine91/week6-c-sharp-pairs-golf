using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Capstone.DAL
{
    class ReservationSqlDAO : IReservationDAO
    {
        private string connectionString;

        public ReservationSqlDAO(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public IList<Reservation> GetReservations(IList<Space> spaces)
        {
            IList<Reservation> reservations = new List<Reservation>();

            foreach (Space space in spaces)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM reservation WHERE space_id = " + space.Id + ";", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = ConvertReaderToTypeReservation(reader);
                        reservations.Add(reservation);
                    }

                }

            }
            return reservations;
        }

        public Reservation ConvertReaderToTypeReservation(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();

            reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
            reservation.SpaceId = Convert.ToInt32(reader["space_id"]);
            reservation.NumberOfAttendees = Convert.ToInt32(reader["number_of_attendees"]);
            reservation.StartDate = Convert.ToDateTime(reader["start_date"]);
            reservation.EndDate = Convert.ToDateTime(reader["end_date"]);
            reservation.ReservedFor = Convert.ToString(reader["reserved_for"]);

            return reservation;
        }

        public bool IsDateAvailable(IList<Reservation> reservations, Space space, DateTime startDate, int numOfDays)
        {
            List<DateTime> newBookingDates = Enumerable.Range(0, numOfDays).Select(i => startDate.AddDays(i)).ToList();
            foreach (Reservation rev in reservations)
            {
                if (rev.SpaceId == space.Id)
                {
                    int bookedDays = (rev.EndDate - rev.StartDate).Days + 1;
                    List<DateTime> bookedRanged = Enumerable.Range(0, bookedDays).Select(i => rev.StartDate.AddDays(i)).ToList();

                    foreach (DateTime booking in bookedRanged)
                    {
                        if (newBookingDates.Contains(booking))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool IsSpaceOperating(Space space, DateTime startDate, int numOfDays)
        {
            List<DateTime> newBookingDates = Enumerable.Range(0, numOfDays).Select(i => startDate.AddDays(i)).ToList();
            if (space.OpenFrom == "")
            {
                return true;
            }

            string[] monthAbbrevOpen = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
            int index = Array.IndexOf(monthAbbrevOpen, space.OpenFrom) + 1;
            DateTime openDay = new DateTime(2020, index, 1);

            string[] monthAbbrevClose = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
            int monthClose = Array.IndexOf(monthAbbrevClose, space.OpenTo) + 1;
            int days = DateTime.DaysInMonth(2020, monthClose);
            DateTime closeDay = new DateTime(2020, monthClose, days);

            int daysOpen = (closeDay - openDay).Days;

            List<DateTime> openRange = Enumerable.Range(0, daysOpen).Select(i => openDay.AddDays(i)).ToList();
            bool openForBooking = newBookingDates.All(openRange.Contains);
            if (openForBooking == false)
            {
                return false;
            }

            return true;
        }

        public bool IsBookingBelowMaxOcc(Space space, int peopleAttending)
        {
            if (peopleAttending > space.MaxOccupancy)
            {
                return false;
            }
            return true;
        }



        public string AddReservationToSql(string spaceIDChosen, string reservedFor, DateTime startDate, int numOfDays, int peopleAttending)
        {
            int spaceID = Convert.ToInt32(spaceIDChosen);
            DateTime endDate = startDate.AddDays(numOfDays);
            Reservation newReservation = new Reservation
            {
                SpaceId = spaceID,
                NumberOfAttendees = peopleAttending,
                StartDate = startDate,
                EndDate = endDate,
                ReservedFor = reservedFor

            };

            int result = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO reservation VALUES (@space_id, @number_of_attendees, " +
                    "@start_date, @end_date, @reserved_for);  " +
                       "SELECT SCOPE_IDENTITY();", conn);
                cmd.Parameters.AddWithValue("@space_id", newReservation.SpaceId);
                cmd.Parameters.AddWithValue("@number_of_attendees", newReservation.NumberOfAttendees);
                cmd.Parameters.AddWithValue("@start_date", newReservation.StartDate);
                cmd.Parameters.AddWithValue("@end_date", newReservation.EndDate);
                cmd.Parameters.AddWithValue("@reserved_for", newReservation.ReservedFor);

                result = Convert.ToInt32(cmd.ExecuteScalar());

            }

            string confirmationID = result.ToString();



            return confirmationID;
        }
        //  public IList<Space> ListSpacesAvailable(IList<Space> spaces, IList<Reservation> reservations, DateTime startDate, int numOfDays, int peopleAttending)
    }
}


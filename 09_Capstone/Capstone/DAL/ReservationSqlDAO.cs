﻿using Capstone.Models;
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
                    bool alreadyBooked = newBookingDates.Intersect(bookedRanged).Any();
                    if (alreadyBooked == true)
                    {
                        return false;
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
            DateTime closeDay = new DateTime(2020, monthClose + 1, 1).AddDays(-1);

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

        public bool AreDatesAvailable(IList<Space> spaces, IList<Reservation> reservations, DateTime startDate, int numOfDays)
        {
            List<DateTime> newBookingDates = Enumerable.Range(0, numOfDays).Select(i => startDate.AddDays(i)).ToList();

            List<int> spaceIDs = new List<int>();
            foreach (Space space in spaces)
            {

            }

            foreach (Reservation rev in reservations)
            {
                int bookedDays = (rev.EndDate - rev.StartDate).Days + 1;
                List<DateTime> bookedRanged = Enumerable.Range(0, bookedDays).Select(i => rev.StartDate.AddDays(i)).ToList();
                bool alreadyBooked = newBookingDates.Intersect(bookedRanged).Any();
                if (alreadyBooked == true)
                {
                    return false;
                }
            }
            for (int index = 0; index < spaces.Count; index++)
            {
                if (spaces[index].OpenFrom == "")
                {
                    return true;
                }
                DateTime openDay = DateTime.Parse(spaces[index].OpenFrom);


                DateTime closeDay = DateTime.Parse(spaces[index].OpenTo);
                int daysOpen = (closeDay - openDay).Days + 31;
                List<DateTime> openRange = Enumerable.Range(0, daysOpen).Select(i => openDay.AddDays(i)).ToList();
                bool openForBooking = newBookingDates.All(openRange.Contains);
                if (openForBooking == true)
                {
                    return true;
                }
            }
            return true;

        }
        //  public IList<Space> ListSpacesAvailable(IList<Space> spaces, IList<Reservation> reservations, DateTime startDate, int numOfDays, int peopleAttending)

    }
}

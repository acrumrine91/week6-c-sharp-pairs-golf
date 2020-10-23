using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace Capstone.DAL
{
    public class SpaceSqlDAO : ISpaceDAO
    {
        private string connectionString;

        public SpaceSqlDAO(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public IList<Space> GetVenueSpaces(int venueNum)
        {
            IList<Space> spaces = new List<Space>();
            int venueID = venueNum + 1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM space WHERE venue_id = " + venueID + ";", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Space space = ConvertReaderToTypeSpace(reader);
                    spaces.Add(space);
                }

            }
            return spaces;
        }

        private Space ConvertReaderToTypeSpace(SqlDataReader reader)
        {
            Space space = new Space();

            space.Id = Convert.ToInt32(reader["id"]);
            space.VenueId = Convert.ToInt32(reader["venue_id"]);
            space.Name = Convert.ToString(reader["name"]);
            space.IsAccessible = (bool)(reader["is_accessible"]);

            string openFrom = Convert.ToString(reader["open_from"]);
            if (openFrom == "")
            {
                space.OpenFrom = openFrom;
            }
            else
            {
                int openFromNum = Convert.ToInt32(openFrom);
                DateTime strDate = new DateTime(2000, openFromNum, 1);
                string monthAbrev= strDate.ToString("MMM");
                space.OpenFrom = monthAbrev;
            }

            string openTo = Convert.ToString(reader["open_to"]);
            if (openTo == "")
            {
                space.OpenTo = openTo;
            }
            else
            {
                int openToNum = Convert.ToInt32(openTo);
                DateTime strDate = new DateTime(2000, openToNum, 1);
                string monthAbrev = strDate.ToString("MMM");
                space.OpenTo = monthAbrev;
            }

            space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
            space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            return space;
        }
    }
}

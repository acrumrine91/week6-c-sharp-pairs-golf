using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

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
            space.OpenFrom = Convert.ToInt32(reader["open_from"]);
            space.OpenTo = Convert.ToInt32(reader["open_to"]);
            space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
            space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);

            return space;
        }
    }
}

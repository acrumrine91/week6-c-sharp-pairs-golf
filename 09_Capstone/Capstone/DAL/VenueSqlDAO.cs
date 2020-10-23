using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class VenueSqlDAO: IVenueDAO
    {
        private string connectionString;

        public VenueSqlDAO(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public IList<Venue> GetVenues()
        {
            IList<Venue> venues = new List<Venue>();

            // maybe we are going to add a trycatch

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM venue;", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Venue venyou = ConvertReaderToTypeVenue(reader);
                    venues.Add(venyou);
                }

            }
            return venues;

        }

        private Venue ConvertReaderToTypeVenue(SqlDataReader reader)
        {
            Venue venyou = new Venue();

            venyou.Id = Convert.ToInt32(reader["id"]);
            venyou.Name = Convert.ToString(reader["name"]);
            venyou.CityId = Convert.ToInt32(reader["city_id"]);
            venyou.Description = Convert.ToString(reader["description"]);

            return venyou;
        }

        //(id, name, city_id, description)
    }
}

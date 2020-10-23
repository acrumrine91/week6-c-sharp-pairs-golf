using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CitySqlDAO : ICityDAO
    {
        private string connectionString;

        public CitySqlDAO(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public City GetVenueCity(int cityNum)
        {

            City cityPlace = new City();
            // maybe we are going to add a trycatch

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM city WHERE id = " + cityNum + ";", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    cityPlace = ConvertReaderToTypeCity(reader);
                }
            }
            return cityPlace;


        }

        private City ConvertReaderToTypeCity(SqlDataReader reader)
        {
            City cityPlace = new City();

            cityPlace.Id = Convert.ToInt32(reader["id"]);
            cityPlace.Name = Convert.ToString(reader["name"]);
            cityPlace.StateAbbreviation = Convert.ToString(reader["state_abbreviation"]);

            return cityPlace;
        }
    }
}

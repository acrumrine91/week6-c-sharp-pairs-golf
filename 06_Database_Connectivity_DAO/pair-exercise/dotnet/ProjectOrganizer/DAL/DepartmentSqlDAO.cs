using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer.DAL
{
    public class DepartmentSqlDAO : IDepartmentDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public DepartmentSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public IList<Department> GetDepartments()
        {
            List<Department> depts = new List<Department>();

            try
            {
                //create connection
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //open connection
                    conn.Open();

                    //create command to send to database
                    SqlCommand cmd = new SqlCommand("SELECT * FROM department;", conn);

                    //execute command
                    SqlDataReader reader = cmd.ExecuteReader();

                    //read each row
                    while (reader.Read())
                    {
                        Department department = ConvertReaderToDepartment(reader);
                        depts.Add(department);
                    }
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred reading Departments.");
                Console.WriteLine(ex.Message);
                throw;
            }
            //return the output
            return depts;

        }

        private Department ConvertReaderToDepartment(SqlDataReader reader)
        {
            Department depts = new Department();

            depts.Id = Convert.ToInt32(reader["department_id"]);
            depts.Name = Convert.ToString(reader["name"]);

            return depts;
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            int id = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO department VALUES (@name);", conn);
                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);

                    cmd.ExecuteNonQuery();

                    // Now print the new Department Id and Department Name
                    cmd = new SqlCommand("SELECT MAX(department_id) FROM department;", conn);
                    id = Convert.ToInt32(cmd.ExecuteScalar());

                    Console.WriteLine($"The new department id is {id}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error saving department.");
                Console.WriteLine(ex.Message);
                throw;
            }
            return id;
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE department SET name = @name FROM department WHERE department_id = @department_id;", conn);
                    cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);
                    cmd.Parameters.AddWithValue("@department_id", updatedDepartment.Id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    Console.WriteLine($"The new department name is @name");
                    if (rowsAffected > 0)
                    {
                        return true; // change was successful
                    }
                    else
                    {
                        return false; // rowsAffected is 0, change was NOT successful
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error saving department.");
                Console.WriteLine(ex.Message);
                throw;
            }


        }

    }
}

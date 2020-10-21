using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public IList<Employee> GetAllEmployees()
        {
            //Delcare the output variable
            List<Employee> employees = new List<Employee>();

            try
            {
                // Create a connection to the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    // Create a command to send to the database
                    SqlCommand cmd = new SqlCommand("SELECT * FROM employee;", conn);

                    // Execute the command
                    SqlDataReader reader = cmd.ExecuteReader();

                    //read each row
                    while (reader.Read())
                    {
                        Employee employee = ConvertReaderToEmployee(reader);
                        employees.Add(employee);
                    }



                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred reading Employee.");
                Console.WriteLine(ex.Message);
                throw;
            }
            return employees;
        }

        private Employee ConvertReaderToEmployee(SqlDataReader reader)
        {
            Employee employee = new Employee();

            employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
            employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
            employee.FirstName = Convert.ToString(reader["first_name"]);
            employee.LastName = Convert.ToString(reader["last_name"]);
            employee.JobTitle = Convert.ToString(reader["job_title"]);
            employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
            employee.Gender = Convert.ToString(reader["gender"]);
            employee.HireDate = Convert.ToDateTime(reader["hire_date"]);



            return employee;
        }

        /// <summary>
        /// Searches the system for an employee by first name or last name.
        /// </summary>
        /// <remarks>The search performed is a wildcard search.</remarks>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns>A list of employees that match the search.</returns>
        public IList<Employee> Search(string firstname, string lastname)
        {

            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // column    // param name  
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Employee WHERE first_name = @first_name AND last_name = @last_name;", conn);
                    // param name    // param value
                    cmd.Parameters.AddWithValue("@first_name", firstname);
                    cmd.Parameters.AddWithValue("@last_name", lastname);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = ConvertReaderToEmployee(reader);
                        employees.Add(employee);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred reading Employees by First and Last Name.");
                Console.WriteLine(ex.Message);
                throw;
            }

            return employees;
        }

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public IList<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // column    // param name  
                    SqlCommand cmd = new SqlCommand("SELECT * FROM employee LEFT JOIN project_employee ON project_employee.employee_id = employee.employee_id WHERE project_id IS NULL", conn);
                    // param name    // param value

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = ConvertReaderToEmployee(reader);
                        employees.Add(employee);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred reading Employees by First and Last Name.");
                Console.WriteLine(ex.Message);
                throw;
            }

            return employees;
        }
    }
}

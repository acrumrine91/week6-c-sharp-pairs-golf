using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class ProjectSqlDAO : IProjectDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public ProjectSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public IList<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();

            try
            {
                // Create a connection to the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Open the connection
                    conn.Open();

                    // Create a command to send to the database
                    SqlCommand cmd = new SqlCommand("SELECT * FROM project;", conn);

                    // Execute the command
                    SqlDataReader reader = cmd.ExecuteReader();

                    //read each row
                    while (reader.Read())
                    {
                        Project project = ConvertReaderToProject(reader);
                        projects.Add(project);
                    }



                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred reading Project.");
                Console.WriteLine(ex.Message);
                throw;
            }
            return projects;
        }

        private Project ConvertReaderToProject(SqlDataReader reader)
        {
            Project project = new Project();

            project.ProjectId = Convert.ToInt32(reader["project_id"]);
            project.Name = Convert.ToString(reader["name"]);
            project.StartDate = Convert.ToDateTime(reader["from_date"]);
            project.EndDate = Convert.ToDateTime(reader["to_date"]);



            return project;
        }






        /// <summary>
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            int id = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO project VALUES (@name, @from_date, @to_date);", conn);
                    cmd.Parameters.AddWithValue("@name ", newProject.Name);
                    cmd.Parameters.AddWithValue("@from_date ", newProject.StartDate);
                    cmd.Parameters.AddWithValue("@to_date ", newProject.EndDate);


                    cmd.ExecuteNonQuery();

                    // Now print the new Project Id
                    cmd = new SqlCommand("SELECT MAX(project_id) FROM project;", conn);
                    id = Convert.ToInt32(cmd.ExecuteScalar());

                    Console.WriteLine($"The new project id is {id}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error saving project.");
                Console.WriteLine(ex.Message);
                throw;
            }
            return id;
        }

    }
}

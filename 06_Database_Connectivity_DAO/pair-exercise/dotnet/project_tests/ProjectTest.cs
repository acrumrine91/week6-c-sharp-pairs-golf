using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace project_tests
{
    class ProjectTest
    {
        [TestClass]
        public class ProjectTests
        {
            protected string ConnectionString { get; } = "Server=.\\SQLEXPRESS;Database=EmployeeDB;Trusted_Connection=True;";

            /// <summary>
            /// The transaction for each test.
            /// </summary>
            private TransactionScope transaction;

            [TestInitialize]
            public void Setup()
            {
                // Begin the transaction
                transaction = new TransactionScope();
            }

            [TestCleanup]
            public void Cleanup()
            {
                // Roll back the transaction
                transaction.Dispose();
            }
            [TestMethod]
            public void CreateProjectShouldIncreaseCountBy1()
            {
                // Arrange
                Project project = new Project();
                project.Name = "Billy Goats Gruff Experience";
                project.StartDate = Convert.ToDateTime("10-20-2020");
                project.EndDate = Convert.ToDateTime("10-21-2020");
                ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);
                int startingRowCount = GetRowCount("project");


                //ACT
                dao.CreateProject(project);

                //Assert

                int newRowCount = GetRowCount("project");
                Assert.AreEqual(startingRowCount + 1, newRowCount);



            }


            [TestMethod]
            public void GetAllProjectsShouldReturnAllNineProjects()
            {
                // Arrange
                ProjectSqlDAO dao = new ProjectSqlDAO(ConnectionString);

                // Act
                IList<Project> dept = dao.GetAllProjects();

                // Assert
                Assert.AreEqual(9, dept.Count);
            }



            protected int GetRowCount(string table)
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count;
                }
            }
        }
    }

}

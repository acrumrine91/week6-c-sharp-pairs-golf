using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace project_tests
{
    [TestClass]
    public class DepartmentTests
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
        public void CreateDepartmentShouldIncreaseCountBy1()
        {
            // Arrange
            Department dept = new Department();
            dept.Name = "Does it even really matter";
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);
            int startingRowCount = GetRowCount("department");


            //ACT
            dao.CreateDepartment(dept);

            //Assert

            int newRowCount = GetRowCount("department");
            Assert.AreNotEqual(startingRowCount, newRowCount);



        }

        [TestMethod]
        public void GetDepartmentsShouldReturnAllFiveDepartments()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);

            // Act
            IList<Department> dept = dao.GetDepartments();

            // Assert
            Assert.AreEqual(5, dept.Count);
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

using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationSqlDAOTests
    {
        protected string ConnectionString { get; } = "Data Source=.\\sqlexpress;Initial Catalog=excelsior_venues;Integrated Security=True";

        private TransactionScope transaction;

        [TestInitialize]
        public void Setup()
        {
            transaction = new TransactionScope();
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }
        
        [TestMethod]
        
        public void GetReservationsShouldReturnAllReservations()
        {
            //Arrange
            ReservationSqlDAO dao = new ReservationSqlDAO(ConnectionString);
            

            //Act
            

            //Assert
            
        }
    }
}

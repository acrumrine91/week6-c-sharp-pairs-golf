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
    public class CitySqlDAOTests
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
        [DataRow(1, 7)]
        [DataRow(2, 4)]
        [DataRow(3, 4)]
        public void GetVenueSpacesShouldReturnAllVenueSpaces(int venueNum, int expectedCount)
        {
            //Arrange
            SpaceSqlDAO dao = new SpaceSqlDAO(ConnectionString);

            //Act
            IList<Space> spaces = dao.GetVenueSpaces(venueNum);

            //Assert
            Assert.AreEqual(expectedCount, spaces.Count);
        }
    }
}

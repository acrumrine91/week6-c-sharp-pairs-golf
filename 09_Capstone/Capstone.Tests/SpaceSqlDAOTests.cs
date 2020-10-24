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
    public class SpaceSqlDAOTests
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
        public void GetVenueSpacesShouldReturnAllVenueSpaces()
        {
            //Arrange
            SpaceSqlDAO dao = new SpaceSqlDAO(ConnectionString);

            //Act
            IList<Space> spaces = dao.GetVenueSpaces(1);

            //Assert
            Assert.AreEqual(7, spaces.Count);
        }
    }
}

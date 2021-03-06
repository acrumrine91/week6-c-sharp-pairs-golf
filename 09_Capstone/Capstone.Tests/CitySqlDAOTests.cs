﻿using Capstone.DAL;
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
        [DataRow(1, "Bona")]
        [DataRow(2, "Srulbury")]
        [DataRow(3, "Yepford")]
        public void GetVenueCityShouldReturnVenueCity(int cityNum, string expectedCityName)
        {
            //Arrange
            CitySqlDAO dao = new CitySqlDAO(ConnectionString);

            //Act
            City cityPlace = dao.GetVenueCity(cityNum);

            //Assert
            Assert.AreEqual(expectedCityName, cityPlace.Name);
        }
    }
}

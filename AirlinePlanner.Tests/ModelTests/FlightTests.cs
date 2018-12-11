using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirlinePlanner.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
 
namespace AirlinePlanner.Tests
{
    [TestClass]
    public class FlightTest : IDisposable
    { 
        public void Dispose()
        {
            Flight.ClearAll();
        }

        public FlightTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=AirlinePlanner_test;";
        }

        [TestMethod]
        public void Saves_SavestoFlightTable_Method()
        {
            
            Flight newFlight = new Flight("0800", "atlanta", "los angeles", "late");
            newFlight.Save();
            List<Flight> resultList = Flight.GetAll();
            Flight result = resultList[0];
            string resultTime = result.GetDepartureTime();

            Assert.AreEqual("0800", resultTime);
        }
    }    
}

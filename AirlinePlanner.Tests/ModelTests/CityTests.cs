using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirlinePlanner.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
 
namespace AirlinePlanner.Tests
{
    [TestClass]
    public class CityTest : IDisposable
    { 
        public void Dispose()
        {
            City.ClearAll();
        }

        public CityTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=AirlinePlanner_test;";
        }

        [TestMethod]
        public void Saves_SavestoCityTable_Method()
        {
            
            City newCity = new City("atlanta");
            newCity.Save();
            List<City> resultList = City.GetAll();
            City result = resultList[0];
            string resultName = result.GetName();
            Assert.AreEqual("atlanta", resultName);
        }
    }    
}

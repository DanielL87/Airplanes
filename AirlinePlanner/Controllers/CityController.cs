using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AirlinePlanner.Models;

namespace AirlinePlanner.Controllers
{
    public class CityController : Controller
    {
        [HttpGet("/cities")]
        public ActionResult Index()
        {
            List<City> allCities = City.GetAll();
            return View(allCities);
        }

        [HttpGet("/cities/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/cities")]
        public ActionResult Create(string name)
        {
            City newCity = new City(name);
            newCity.Save();
            List<City> allCities = City.GetAll();
            return View("Index", allCities);
        }

        [HttpGet("/cities/{id}")]
        public ActionResult Show(int id)
        {
        City foundCity = City.Find(id);
        List<Flight> foundFlights = foundCity.GetFlights();
        Dictionary<string, object> myDic = new Dictionary<string, object> ();
        myDic.Add("city", foundCity);
        myDic.Add("flights", foundFlights);
        return View(myDic);
        }
        
    }
}

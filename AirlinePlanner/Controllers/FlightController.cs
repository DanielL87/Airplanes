using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AirlinePlanner.Models;
using System;

namespace AirlinePlanner.Controllers
{
    public class FlightController : Controller
    {
        [HttpGet("/flights")]
        public ActionResult Index()
        {
            List<Flight> allFlights = Flight.GetAll();
            return View(allFlights);
        }

        [HttpGet("flights/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/flights")]
        public ActionResult Create(string departureTime, string departureCity, string arrivalCity, string status)
        {
            Flight newFlight = new Flight(departureTime, departureCity, arrivalCity, status);
            newFlight.Save();
            List<Flight> allFlights = Flight.GetAll();
            return View("Index", allFlights);
        }

        [HttpGet("/flights/{id}")]
        public ActionResult Show(int id)
        {
            Flight foundFlight = Flight.Find(id);
            List<City> foundCities = foundFlight.GetCities();
            List<City> allCities = City.GetAll();
            Dictionary<string, object> myDic = new Dictionary<string, object> ();
            myDic.Add("flight", foundFlight);
            myDic.Add("cities", foundCities);
            myDic.Add("allCities", allCities);
            return View(myDic);   
        }

        [HttpPost("/flights/{id}")]
        public ActionResult AddCityCreate(int id, int cityId)
        {
            Flight foundFlight = Flight.Find(id);
            City foundCity = City.Find(cityId);
            foundFlight.AddCity(foundCity);
            return RedirectToAction("Show");   
        }
    }
}
   
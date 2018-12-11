using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Models
{
    public class Flight
    {
        private int _id;
        private string _departureTime;
        private string _departureCity;
        private string _arrivalCity;
        private string _status;

        public Flight(string departureTime, string departureCity, string arrivalCity, string status, int id = 0)
        {
            _departureTime = departureTime;
            _departureCity = departureCity;
            _arrivalCity = arrivalCity;
            _status = status;
            _id = id;
        }
        public int GetId()
        {
         return _id;   
        }

        public string GetDepartureTime()
        {
            return _departureTime;
        }

        public string GetDepartureCity()
        {
            return _departureCity;
        }

        public string GetArrivalCity()
        {
            return _arrivalCity;
        }

        public string GetStatus()
        {
            return _status;
        }

        //Saves
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (departureTime, departureCity, arrivalCity, status) VALUES (@departureTime, @departureCity, @arrivalCity, @status);";

            MySqlParameter departureTime = new MySqlParameter();
            departureTime.ParameterName = "@departureTime";
            departureTime.Value = this._departureTime;
            cmd.Parameters.Add(departureTime);

            MySqlParameter departureCity = new MySqlParameter();
            departureCity.ParameterName = "@departureCity";
            departureCity.Value = this._departureCity;
            cmd.Parameters.Add(departureCity);

            MySqlParameter arrivalCity = new MySqlParameter();
            arrivalCity.ParameterName = "@arrivalCity";
            arrivalCity.Value = this._arrivalCity;
            cmd.Parameters.Add(arrivalCity);

            MySqlParameter status = new MySqlParameter();
            status.ParameterName = "@status";
            status.Value = this._status;
            cmd.Parameters.Add(status);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        //Getters

        public static List<Flight> GetAll()
        {
            List<Flight> allFlights = new List<Flight> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string departureTime = rdr.GetString(1);
                string departureCity = rdr.GetString(2);
                string arrivalCity = rdr.GetString(3);
                string status = rdr.GetString(4);

                Flight newFlight = new Flight(departureTime, departureCity, arrivalCity, status, id);
                allFlights.Add(newFlight);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allFlights;
        }


            //Clears and Overrides

        public static void ClearAll()
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();
                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"DELETE FROM flights;";
                cmd.ExecuteNonQuery();

                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
        public override bool Equals(System.Object otherFlight)
            {
                if (!(otherFlight is Flight))
                {
                    return false;
                }
                else
                {
                    Flight newFlight = (Flight) otherFlight;
                    bool descriptionEquality = (this.GetDepartureTime() == newFlight.GetDepartureTime());
                    return (descriptionEquality);
                }
            }

        public override int GetHashCode()
        {
            return this.GetDepartureTime().GetHashCode();
        }

        public static Flight Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int FlightId = 0;
            string FlightDepartureTime = "";
            string FlightDepartureCity = "";
            string FlightArrivalCity = "";
            string FlightStatus = "";
            while(rdr.Read())
            {
                FlightId = rdr.GetInt32(0);
                FlightDepartureTime = rdr.GetString(1);
                FlightArrivalCity = rdr.GetString(2);
                FlightDepartureCity = rdr.GetString(3);
                FlightStatus = rdr.GetString(4);
            }
            Flight newFlight = new Flight(FlightDepartureTime, FlightArrivalCity, FlightDepartureCity, FlightStatus, FlightId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newFlight;
        }       

        public void AddCity(City newCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@cityId, @flightsId);";
            MySqlParameter flights_id = new MySqlParameter();
            flights_id.ParameterName = "@flightsId";
            flights_id.Value = _id;
            cmd.Parameters.Add(flights_id);
            MySqlParameter City_id = new MySqlParameter();
            City_id.ParameterName = "@cityId";
            City_id.Value = newCity.GetId();
            cmd.Parameters.Add(City_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    public List<City> GetCities()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT cities.* FROM flights
            JOIN cities_flights ON (flights.id = cities_flights.flight_id)
            JOIN cities ON (cities_flights.city_id = cities.id)
            WHERE flights.id = @flightId;";
        MySqlParameter flightIdParameter = new MySqlParameter();
        flightIdParameter.ParameterName = "@flightId";
        flightIdParameter.Value = _id;
        cmd.Parameters.Add(flightIdParameter);
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        List<City> Cities = new List<City>{};
        while(rdr.Read())
        {
          int CityId = rdr.GetInt32(0);
          string CityName = rdr.GetString(1);
          City newCity = new City(CityName, CityId);
          Cities.Add(newCity);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return Cities;
    }



    }
}

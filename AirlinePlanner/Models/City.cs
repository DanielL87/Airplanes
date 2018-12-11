using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Models
{
    public class City
    {
        private int _id;
        private string _name;


    public City(string name, int id = 0)
    {
    _name = name;
    _id = id;
    }   

    public string GetName()
    {
        return _name;
    }

    public int GetId()
    {
        return _id;
    }

     //Saves
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

         //Getters

        public static List<City> GetAll()
            {
            List<City> allCitys = new List<City> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);

                City newCity = new City(name, id);
                allCitys.Add(newCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCitys;
            }

            //Clears and Overrides

            public static void ClearAll()
                {
                    MySqlConnection conn = DB.Connection();
                    conn.Open();
                    var cmd = conn.CreateCommand() as MySqlCommand;
                    cmd.CommandText = @"DELETE FROM cities;";
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    if (conn != null)
                    {
                        conn.Dispose();
                    }
                }

            public override bool Equals(System.Object otherCity)
                {
                    if (!(otherCity is City))
                    {
                        return false;
                    }
                    else
                    {
                        City newCity = (City) otherCity;
                        bool descriptionEquality = (this.GetName() == newCity.GetName());
                        return (descriptionEquality);
                    }
                }

                public override int GetHashCode()
                {
                    return this.GetName().GetHashCode();
                }       

                 public static City Find(int id)
                {
                    MySqlConnection conn = DB.Connection();
                    conn.Open();
                    var cmd = conn.CreateCommand() as MySqlCommand;
                    cmd.CommandText = @"SELECT * FROM cities WHERE id = (@searchId);";
                    MySqlParameter searchId = new MySqlParameter();
                    searchId.ParameterName = "@searchId";
                    searchId.Value = id;
                    cmd.Parameters.Add(searchId);
                    var rdr = cmd.ExecuteReader() as MySqlDataReader;
                    int CityId = 0;
                    string CityName = "";

                    while(rdr.Read())
                    {
                        CityId = rdr.GetInt32(0);
                        CityName = rdr.GetString(1);
                    }

                    City newCity = new City(CityName, CityId);
                    conn.Close();
                    if (conn != null)
                    {
                        conn.Dispose();
                    }
                    return newCity;
                }

            public void AddFlight(Flight newFlight)
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();
                var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@cityId, @flightId);";

                MySqlParameter flight_id = new MySqlParameter();
                flight_id.ParameterName = "@flightId";
                flight_id.Value = newFlight.GetId();
                cmd.Parameters.Add(flight_id);

                MySqlParameter city_id = new MySqlParameter();
                city_id.ParameterName = "@cityId";
                city_id.Value = _id;
                cmd.Parameters.Add(city_id);

                cmd.ExecuteNonQuery();
                conn.Close();
                if (conn != null)
                {
                    conn.Dispose();
                }
            }    

            public List<Flight> GetFlights()
            {
                MySqlConnection conn = DB.Connection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT flights.* FROM cities
                    JOIN cities_Flights ON (cities.id = cities_flights.city_id)
                    JOIN flights ON (cities_flights.flight_id = flights.id)
                    WHERE cities.id = @cityId;";

                MySqlParameter cityIdParameter = new MySqlParameter();
                cityIdParameter.ParameterName = "@cityId";
                cityIdParameter.Value = _id;
                cmd.Parameters.Add(cityIdParameter);
                MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
                List<Flight> flights = new List<Flight>{};
                while(rdr.Read())
                {
                int id = rdr.GetInt32(0);
                string departureTime = rdr.GetString(1);
                string departureCity = rdr.GetString(2);
                string arrivalCity = rdr.GetString(3);
                string status = rdr.GetString(4);

                Flight newFlight = new Flight(departureTime, departureCity, arrivalCity, status, id);
                flights.Add(newFlight);
                }
                conn.Close();
                if (conn != null)
                {
                conn.Dispose();
                }
                return flights;
            }

    }
}    

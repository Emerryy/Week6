using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class CityDAO
    {
        private string connectionString;

        //private string sqlGetVenueInfoById = "  SELECT v.name, v.description, c.name, s.name, ca.name FROM category_venue cv "
        //            + "JOIN category ca ON ca.id = cv.category_id"
        //            + "JOIN venue v ON v.id = cv.venue_id"
        //            + "JOIN city c ON c.id = v.city_id"
        //            + "JOIN state s ON s.abbreviation = c.state_abbreviation"
        //            + "WHERE v.id = @id;";

        private string sqlGetVenueInfoById = " SELECT name FROM city"
            + "JOIN category ca ON ca.id = cv.category_id"
            + "JOIN venue v ON v.id = cv.venue_id"
            + "JOIN city c ON c.id = v.city_id"
            + "JOIN state s ON s.abbreviation = c.state_abbreviation"
            + "WHERE v.id = @id;";

        public CityDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private City ConvertReaderToCity(SqlDataReader reader)
        {
            City city = new City();

            city.CityId = Convert.ToInt32(reader["id"]);
            city.Name = Convert.ToString(reader["name"]);
            city.StateAbbreviation = Convert.ToString(reader["state_abbreviation"]);

            return city;
        }
    }
}

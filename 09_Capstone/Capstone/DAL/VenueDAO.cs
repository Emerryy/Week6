using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class VenueDAO : IVenueDAO
    {
        // NOTE: No Console.ReadLine or Console.WriteLine in this class

        private string connectionString;

        private string sqlGetVenueInfoById = "  SELECT v.name, v.description, c.name, s.name, ca.name FROM category_venue cv "
                                            + "JOIN category ca ON ca.id = cv.category_id"
                                            + "JOIN venue v ON v.id = cv.venue_id"
                                            + "JOIN city c ON c.id = v.city_id"
                                            + "JOIN state s ON s.abbreviation = c.state_abbreviation"
                                            + "WHERE v.id = @id;";

        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Venue> GetVenuesInAlphaOrder()
        {
            List<Venue> venues = new List<Venue>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT name, id FROM venue ORDER BY name ASC;", conn);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venue = ConvertReaderToVenue(reader);
                        venues.Add(venue);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(ex.Message);

            }

            return venues;
        }


        public IList<Venue> GetVenueInfoByID(int venueId)
        {
            IList<Venue> venueList = new List<Venue>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlGetVenueInfoById, conn);
                    cmd.Parameters.AddWithValue("@id", venueId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venue = ConvertReaderToVenue(reader);
                        venueList.Add(venue);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(ex.Message);

            }
            return 0;
        }

        private Venue ConvertReaderToVenue(SqlDataReader reader)
        {
            Venue venue = new Venue();

            venue.Name = Convert.ToString(reader["name"]);
            venue.ID = Convert.ToInt32(reader["id"]);
            venue.CityID = Convert.ToInt32(reader["city_id"]);
            venue.Description = Convert.ToString(reader["description"]);

            return venue;
        }

    }
}

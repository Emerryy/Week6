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

        private string sqlGetVenueInfoById = "  SELECT v.name AS VenueName, v.description AS VenueDescription, c.name AS CityName, s.name AS StateName, ca.name AS CategoryName FROM category_venue cv "
                                            + "JOIN category ca ON ca.id = cv.category_id "
                                            + "JOIN venue v ON v.id = cv.venue_id " 
                                            + "JOIN city c ON c.id = v.city_id "
                                            + "JOIN state s ON s.abbreviation = c.state_abbreviation "
                                            + "WHERE v.id = @id; ";

        private string sqlLookForVenueID = " SELECT COUNT(*) FROM venue WHERE id = @id";

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

                    SqlCommand cmd = new SqlCommand("SELECT * FROM venue ORDER BY name ASC;", conn);


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


        public List<ListedVenue> GetVenueInfoByID(int venueId)
        {
            List<ListedVenue> venueInfo = new List<ListedVenue>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlGetVenueInfoById, conn);
                    cmd.Parameters.AddWithValue("@id", venueId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<string> categories = new List<string>();

                    while (reader.Read())
                    {
                        

                        ListedVenue lV = new ListedVenue();
                        lV.VenueName = Convert.ToString(reader["VenueName"]);
                        lV.VenueDescription = Convert.ToString(reader["VenueDescription"]);
                        lV.CityName = Convert.ToString(reader["CityName"]);
                        lV.StateName = Convert.ToString(reader["StateName"]);
                        categories.Add(Convert.ToString(reader["CategoryName"]));

                        lV.CategoryName = categories;

                        venueInfo.Add(lV);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(ex.Message);
            }

            return venueInfo;

        }

        public bool IsVenueIDValid(int venueId)
        {
            bool valid = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlLookForVenueID, conn);
                    cmd.Parameters.AddWithValue("@id", venueId);
                    int doesItExist = Convert.ToInt32(cmd.ExecuteScalar());
                    if (doesItExist == 1)
                    {
                        valid = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(ex.Message);
            }

            return valid;
        }

        public Venue ConvertReaderToVenue(SqlDataReader reader)
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

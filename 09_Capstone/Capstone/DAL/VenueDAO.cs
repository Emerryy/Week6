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

        public VenueDAO (string connectionString)
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

                    while(reader.Read())
                    {
                        Venue venue = new Venue();
                        venue.Name = Convert.ToString(reader["name"]);
                        venue.ID = Convert.ToInt32(reader["id"]);
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

        //private Venue ConvertReaderToVenue(SqlDataReader reader)
        //{
        //    Venue venue = new Venue();
        //}
    }
}

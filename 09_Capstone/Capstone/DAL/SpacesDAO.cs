using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SpacesDAO //:ISpacesDAO
    {
        private string connectionString;

        private string sqlLookForSpaceId = "SELECT COUNT(*) FROM space WHERE id = @id";
        public SpacesDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Space> GetSpacesInfoFromVenueId(int venueId)
        {
            List<Space> venueSpaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM space WHERE venue_id = @venueId ORDER BY id ASC;", conn);
                    cmd.Parameters.AddWithValue("@venueId", venueId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Space space = ConvertReaderToSpace(reader);
                        venueSpaces.Add(space);
                    }
                }
            }
            catch (SqlException)
            {
                throw;

            }

            return venueSpaces;
        }

        public bool IsSpaceIDValid(int spaceId)
        {
            bool valid = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlLookForSpaceId, conn);
                    cmd.Parameters.AddWithValue("@id", spaceId);
                    int doesItExist = Convert.ToInt32(cmd.ExecuteScalar());
                    if (doesItExist == 1)
                    {
                        valid = true;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return valid;
        }

        public Space ConvertReaderToSpace(SqlDataReader reader)
        {
            Space space = new Space();

            space.Id = Convert.ToInt32(reader["id"]);
            space.VenueId = Convert.ToInt32(reader["venue_id"]);
            space.Name = Convert.ToString(reader["name"]);
            space.IsAccessible = Convert.ToBoolean(reader["is_accessible"]);
            if (reader.IsDBNull(4))
            {
                space.OpenFrom = -1;
            }
            else
            {
                space.OpenFrom = Convert.ToInt32(reader["open_from"]);
            }
            if (reader.IsDBNull(5))
            {
                space.OpenTo = -1;
            }
            else
            {
                space.OpenTo = Convert.ToInt32(reader["open_to"]);
            }
            space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
            space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);

            return space;
        }
    }
}

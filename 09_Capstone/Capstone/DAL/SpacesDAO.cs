using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SpacesDAO //:ISpacesDAO
    {


        private Space ConvertReaderToSpace(SqlDataReader reader)
        {
            Space space = new Space();

            space.Id = Convert.ToInt32(reader["id"]);
            space.VenueId = Convert.ToInt32(reader["venue_id"]);
            space.Name = Convert.ToString(reader["name"]);
            space.IsAccessible = Convert.ToBoolean(reader["is_accessible"]);
            space.OpenFrom = Convert.ToDateTime(reader["open_from"]);
            space.OpenTo = Convert.ToDateTime(reader["open_to"]);
            space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
            space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);

            return space;

        }
    }
}

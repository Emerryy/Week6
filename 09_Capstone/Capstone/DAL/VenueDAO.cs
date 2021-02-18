using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public class VenueDAO
    {
        // NOTE: No Console.ReadLine or Console.WriteLine in this class

        private string connectionString;

        public VenueDAO (string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}

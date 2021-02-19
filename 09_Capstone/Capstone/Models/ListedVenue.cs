using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class ListedVenue
    {
        public string VenueName { get; set; }

        public string VenueDescription { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public List<string> CategoryName { get;

            set; 
        }

    }
}

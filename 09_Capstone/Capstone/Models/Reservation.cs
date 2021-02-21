using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int SpaceId { get; set; }
        public int NumberOfAttendees { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReservedFor { get; set; }
        public string VenueName { get; set; }
        public string SpaceName { get; set; }
        public int NumberOfDaysReserved { get; set; }
        public decimal DailyRate { get; set; }
        public decimal TotalCost { get; set; }
    }
}

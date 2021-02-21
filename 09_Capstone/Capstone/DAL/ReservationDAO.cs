using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO
    {
        private string connectionString;
        private string sqlAddNewReservation =
            "INSERT INTO reservation(space_id, number_of_attendees, start_date, end_date, reserved_for) "
            + " VALUES (@spaceid, @attendees, @startdate, @enddate, @reservedfor "
            + " SELECT SCOPE_IDENTITY();";
        private string sqlCheckBookedDates =
            "SELECT start_date, end_date, space_id FROM reservation "
            + "WHERE space_id = @spaceid";



        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Reservation AddNewReservation(Reservation newReservation)
        {
            int reservationID = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlAddNewReservation, conn);
                    cmd.Parameters.AddWithValue("@spaceid", newReservation.SpaceId);
                    cmd.Parameters.AddWithValue("@attendees", newReservation.NumberOfAttendees);
                    cmd.Parameters.AddWithValue("@startdate", newReservation.StartDate);
                    cmd.Parameters.AddWithValue("@enddate", newReservation.EndDate);
                    cmd.Parameters.AddWithValue("@reservedfor", newReservation.ReservedFor);

                    reservationID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException) { }

            return new Reservation
            {
                ReservationId = reservationID,
                SpaceId = newReservation.SpaceId,
                NumberOfAttendees = newReservation.NumberOfAttendees,
                StartDate = newReservation.StartDate,
                EndDate = newReservation.EndDate,
                ReservedFor = newReservation.ReservedFor
            };
        }

        public List<DateTime> GetBookedDates(int spaceId)
        {
            List<DateTime> bookedDates = new List<DateTime>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlCheckBookedDates, conn);
                    cmd.Parameters.AddWithValue("@spaceid", spaceId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Reservation bD = new Reservation();
                        
                        bD.StartDate = Convert.ToDateTime(reader["start_date"]);
                        bD.EndDate = Convert.ToDateTime (reader["end_date"]);
                       

                        bookedDates.Add(bD);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return bookedDates;

        }

        public Reservation ConvertReaderToReservation(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();

            reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
            reservation.SpaceId = Convert.ToInt32(reader["space_id"]);
            reservation.NumberOfAttendees = Convert.ToInt32(reader["number_of_attendees"]);
            reservation.StartDate = Convert.ToDateTime(reader["start_date"]);
            reservation.EndDate = Convert.ToDateTime(reader["end_date"]);
            reservation.ReservedFor = Convert.ToString(reader["reserved_for"]);

            return reservation;
        }

        public int GetCountOfReservations()
        {
            int counts = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                //Create a new test department
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from reservation", conn);
                counts = (int)cmd.ExecuteScalar();
            }

            return counts;
        }
    }
}

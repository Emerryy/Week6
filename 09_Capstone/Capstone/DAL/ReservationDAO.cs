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
        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public void AddNewReservation()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO reservation ", conn);
                }
            }

            resDAO.ConvertReaderToReservation
        }

        public void CheckExistingReservations()
        {
            List<Reservation> existingReservations = new List<Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM reservation", conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Reservation res = ConvertReaderToReservation(reader);
                        existingReservations.Add(res);
                    }
                }
            }
            catch
            {

            }
            return existingReservations;
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
    }
}

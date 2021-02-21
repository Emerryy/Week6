using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationDAOTest : ParentTest
    {
        [TestMethod]
        public void Can_create_Reservation_object()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsNotNull(resDAO);
        }

        [TestMethod]
        [DataRow("SELECT TOP 1 * FROM reservation;", 1)]
        [DataRow("SELECT TOP 3 * FROM reservation;", 3)]
        public void DoesReaderConvertToReservation(string sqlCommand, int expectedRows)
        {
            //Arrange
            List<Reservation> resList = new List<Reservation>();


            //Act
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Reservation tempRes = resDAO.ConvertReaderToReservation(reader);
                        resList.Add(tempRes);
                    }
                }
            }
            catch (SqlException ex) { }

            //Assert
            Assert.IsNotNull(resList[0]);
            Assert.AreEqual(expectedRows, resList.Count);
        }

        [TestMethod()]
        public void Create_Reservation_Test()
        {
            DateTime first = DateTime.Today;
            DateTime last = first.AddDays(2);
            int originalCount = GetCountOfReservations();
            Reservation newRes = new Reservation
            {
                //ReservationId = originalCount + 1,
                SpaceId = 1,
                NumberOfAttendees = 5,
                StartDate = first,
                EndDate = last,
                ReservedFor = "Jimmy Hoffa",

            };
            ReservationDAO dao = new ReservationDAO(connectionString);

            Reservation res = dao.AddNewReservation(newRes);

            string result = res.ReservedFor; 
            Assert.AreEqual(result, "Jimmy Hoffa");
            Assert.AreEqual(originalCount + 1, GetCountOfReservations());
        }

        private int GetCountOfReservations()
        {
            throw new NotImplementedException();
        }
    }
}

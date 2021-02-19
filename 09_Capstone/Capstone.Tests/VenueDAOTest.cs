using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Capstone.Tests
{
    [TestClass]
    public class VenueDAOTest : ParentTest
    {
        [TestMethod]
        public void Can_create_Venue_object()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsNotNull(venueDAO);

        }

        [TestMethod]
        [DataRow("SELECT TOP 1 * FROM venue ORDER BY name ASC;", 1)]
        [DataRow("SELECT TOP 3 * FROM venue ORDER BY name DESC;", 3)]
        public void DoesReaderConvertToVenue(string sqlCommand, int expectedRows)
        {
            //Arrange
            List<Venue> venuesList = new List<Venue>();
            

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
                        Venue tempVenue = venueDAO.ConvertReaderToVenue(reader);
                        venuesList.Add(tempVenue);
                    }
                }
            }
            catch (SqlException ex) { }

            //Assert
            Assert.IsNotNull(venuesList[0]);
            Assert.AreEqual(expectedRows, venuesList.Count);
        }


        [TestMethod]
        [DataRow(1)]
        [DataRow(4)]
        public void CanYouGetVenueInfoByID()
        {

        }
    }
}

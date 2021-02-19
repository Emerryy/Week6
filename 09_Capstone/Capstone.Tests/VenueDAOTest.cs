using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [DataRow("SELECT TOP 1 * FROM venue ORDER BY name ASC;", 6)]
        [DataRow("SELECT TOP 1 * FROM venue ORDER BY name DESC;", 13)]
        public void DoesReaderConvertToVenue(string sqlCommand, int expectedId)
        {
            //Arrange
            Venue tempVenue = new Venue();

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
                        tempVenue = venueDAO.ConvertReaderToVenue(reader);
                    }
                }
            }
            catch (SqlException ex) { }

            //Assert
            Assert.IsNotNull(tempVenue);
            Assert.AreEqual(expectedId, tempVenue.ID);

        }
    }
}

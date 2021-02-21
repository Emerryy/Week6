using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class SpaceDAOTest : ParentTest
    {
        private TransactionScope trans;


        [TestMethod]
        public void Can_create_Space_object()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsNotNull(spaceDAO);

        }

        [TestMethod]
        [DataRow("SELECT TOP 1 * FROM space ORDER BY name ASC;", 1)]
        [DataRow("SELECT TOP 3 * FROM space ORDER BY name DESC;", 3)]
        public void DoesReaderConvertToSpace(string sqlCommand, int expectedRows)
        {
            //Arrange
            List<Space> spaceList = new List<Space>();


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
                        Space tempSpace = spaceDAO.ConvertReaderToSpace(reader);
                        spaceList.Add(tempSpace);
                    }
                }
            }
            catch (SqlException ex) { }

            //Assert
            Assert.IsNotNull(spaceList[0]);
            Assert.AreEqual(expectedRows, spaceList.Count);
        }

        [TestMethod]
        [DataRow("SELECT * FROM space WHERE venue_id = 6;", 4)]
        [DataRow("SELECT * FROM space WHERE venue_id = 8;", 6)]

        public void Can_Get_Spaces_By_VenueID(string sqlCommand, int expectedRows)
        {
            List<Space> spaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Space tempSpace = spaceDAO.ConvertReaderToSpace(reader);
                        spaces.Add(tempSpace);
                    }
                }
            }
            catch (SqlException ex)
            {

                //Assert
                Assert.IsNotNull(spaces[0]);
                Assert.AreEqual(expectedRows, spaces.Count);
            }
        }

        //[TestMethod]
    }
}
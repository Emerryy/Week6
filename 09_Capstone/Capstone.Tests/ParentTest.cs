using Capstone.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class ParentTest
    {
        private TransactionScope trans;

        protected string connectionString;
        protected VenueDAO venueDAO;
        protected SpacesDAO spaceDAO;
        protected ReservationDAO resDAO;

        public ParentTest()
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            connectionString = configuration.GetConnectionString("Project");
        }

        [TestInitialize]
        public void Setup()
        {
            trans = new TransactionScope();
            venueDAO = new VenueDAO(connectionString);
            spaceDAO = new SpacesDAO(connectionString);
            resDAO = new ReservationDAO(connectionString);
        }

        [TestCleanup]
        public void Reset()
        {
            trans.Dispose();
        }

    }
}

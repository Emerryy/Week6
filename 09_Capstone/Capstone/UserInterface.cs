using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class UserInterface
    {
        //ALL Console.ReadLine and WriteLine in this class
        //NONE in any other class

        private string connectionString;
        private VenueDAO venueDAO;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
        }

        public void Run()
        {
            bool done = false;
            while (!done)
            {
                string result = DisplayMainMenu();

                switch (result.ToLower())
                {
                    case "1":
                        string input = ListVenuesMenu();
                        break;

                    case "Q":
                        Console.WriteLine("Thanks!");
                        done = true;
                        break;

                    default:
                        Console.WriteLine("Please make a valid selection.");
                        break;
                }
            }
            return;
        }
        public string DisplayMainMenu()
        {
            Console.WriteLine("Welcome to Exscelsior Venues! Sorry if I spelled our own name wrong");
            Console.WriteLine("1) List Venues");
            Console.WriteLine("Q) Quit");
            return Console.ReadLine();
        }

        public string ListVenuesMenu()
        {
            

            IList<Venue> venues = venueDAO.GetVenuesInAlphaOrder();

            foreach(Venue item in venues)
            {
                Console.WriteLine($"{item.Name} Venue ID- {item.ID} ");
            }
            Console.WriteLine();
            Console.WriteLine("Please input the venue ID you'd like to view");
            return Console.ReadLine();

        }

    }
}

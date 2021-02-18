using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Capstone
{
    public class UserInterface
    {
        //ALL Console.ReadLine and WriteLine in this class
        //NONE in any other class

        private string connectionString;
        private VenueDAO venueDAO;
        private SpacesDAO spaceDAO;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
            spaceDAO = new SpacesDAO(connectionString);
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
                        string venueMenuInput = ListVenuesMenu();
                        string venueInfoMenuInput = GetVenueInfo(int.Parse(venueMenuInput));
                        VenueSpacesOrReservationMenu(venueInfoMenuInput, int.Parse(venueMenuInput));

                        break;
                    case "q":
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

            foreach (Venue item in venues)
            {
                Console.WriteLine($"{item.Name}".PadRight(35) + $" Venue ID- {item.ID}".PadRight(4));
            }
            Console.WriteLine();
            Console.WriteLine("Please input the venue ID you'd like to view");
            return Console.ReadLine();
        }

        public string GetVenueInfo(int venueId)
        {
            //Venue venue = venueDAO.GetVenueInfoByID(venueId);          //todo fix venue info by ID
            //return "";
            Console.WriteLine("Pretend this is info about a venue!");
            Console.WriteLine();

            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1) View Spaces");
            Console.WriteLine("2) Search for Reservation");
            Console.WriteLine("R) Return to Previous Screen");
            return Console.ReadLine();
        }

        public void VenueSpacesOrReservationMenu(string venueInfoMenuInput, int venueId)
        {
            bool done = false;
            while (!done)
            {
                switch (venueInfoMenuInput.ToLower())
                {
                    case "1":
                        Console.WriteLine();
                        string spaceInfoInput = PrintSpaceInfo(venueId);
                        string spaceTime = ReserveASpaceTime();
                        string spaceDays = ReserveASpaceDays();
                        string spaceAttendees = ReserveASpaceAttendees();

                        break;
                    case "2":
                        //2) Search for Reservation
                        break;
                    case "r":
                        Console.WriteLine("Returning to previous page...");
                        done = true;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid input.");
                        break;
                }
            }
        }

        public string PrintSpaceInfo(int venueId)
        {
            IList<Space> spacesList = spaceDAO.GetSpacesInfoFromVenueId(venueId);
            foreach (Space space in spacesList)
            {

                if (space.OpenFrom == -1 || space.OpenTo == -1)
                {
                    Console.WriteLine($"#{space.Id} {space.Name} Open Year Round {space.DailyRate} {space.MaxOccupancy} ");
                }
                else
                {
                    string openFrom = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(space.OpenFrom);
                    string openTo = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(space.OpenTo);
                    Console.WriteLine($"#{space.Id} {space.Name} {openFrom} {openTo} {space.DailyRate} {space.MaxOccupancy} ");
                }
            }

            Console.WriteLine();
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1) Reserve a Space");
            Console.WriteLine("R) Return to Previous Screen");
            return Console.ReadLine();

        }

        public string ReserveASpaceTime()
        {
            Console.Write("When do you need the space? ");
            return Console.ReadLine();
        }

        public string ReserveASpaceDays()
        {
            Console.Write("How many days will you need the space? ");
            return Console.ReadLine();
        }

        public string ReserveASpaceAttendees() 
        {
            Console.Write("How many people will be in attendance? ");
            return Console.ReadLine();
        }

        public void GetSpaceInfoByVenue(int venueId)
        {
            Console.WriteLine();
        }

    }
}

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
        //private DateTime firstDay;
        //private int daysNeeded;
        //private int numGuests;


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
                        string venueIdInput = ListVenuesMenu();
                        string venueInfoMenuInput = GetVenueInfo(int.Parse(venueIdInput));
                        VenueSpacesOrReservationMenu(venueInfoMenuInput, int.Parse(venueIdInput));

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
            List<ListedVenue> listedVenues = venueDAO.GetVenueInfoByID(venueId);


            ListedVenue lv2 = listedVenues[listedVenues.Count - 1];

            Console.WriteLine($"{lv2.VenueName}");
            Console.WriteLine($"{lv2.CityName}, {lv2.StateName}");
            foreach (string cat in lv2.CategoryName)
            {
                Console.WriteLine($"{cat}");
            }
            Console.WriteLine();
            Console.WriteLine($"{lv2.VenueDescription}");

            //Console.WriteLine("Pretend this is info about a venue!");
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

                        break;
                    case "2":
                        DateTime reserveDate = ReserveASpaceTime();
                        int reserveDays = ReserveASpaceDays();
                        int reserveGuests = ReserveASpaceAttendees();
                        SpaceReservationMenu(venueId, reserveDate, reserveDays, reserveGuests);
                        //SpaceReservationMenu(venueId, reserveDate, reserveDays, reserveGuests);
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

        public void SpaceReservationMenu(int venueId, DateTime reserveDate, int reserveDays, int reserveGuests)
        {
            IList<Space> spaces = CheckAvailSpace(venueId, reserveDate, reserveDays, reserveGuests);
            Console.WriteLine("These spaces are available based on your needs:");
            foreach (Space space in spaces)
            {
                Console.WriteLine($"{space.Id} {space.Name} ${space.DailyRate} {space.MaxOccupancy} {space.IsAccessible} ${space.DailyRate * reserveDays} ");
            }

            Console.WriteLine("Which space would you like to reserve? Enter 0 to cancel. ");
            string spaceIdInput = Console.ReadLine();
            Console.WriteLine("Who is this reservation for?");
            string resNameInput = Console.ReadLine();


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

        public DateTime ReserveASpaceTime()
        {
            Console.Write("When do you need the space? Please use a MM/DD/YYYY format ");
            DateTime firstDay = DateTime.Parse(Console.ReadLine());

            return firstDay;
        }

        public int ReserveASpaceDays()
        {
            Console.Write("How many days will you need the space? ");
            int daysNeeded = int.Parse(Console.ReadLine());

            return daysNeeded;
        }

        public int ReserveASpaceAttendees()
        {
            Console.Write("How many people will be in attendance? ");

            int numGuests = int.Parse(Console.ReadLine());
            return numGuests;
        }

        public IList<Space> CheckAvailSpace(int venueId, DateTime firstDay, int daysNeeded, int numGuests)
        {
            IList<Space> spacesList = spaceDAO.GetSpacesInfoFromVenueId(venueId);
            DateTime endDate = firstDay.AddDays(daysNeeded);
            IList<Space> matchSpaces = new List<Space>();

            int endMonth = endDate.Month;
            int startMonth = firstDay.Month;

            foreach (Space space in spacesList)
            {
                if (space.OpenFrom == -1 && numGuests <= space.MaxOccupancy)
                {
                    matchSpaces.Add(space);
                    //Console.WriteLine($"{space.Id} {space.Name} ${space.DailyRate} {space.MaxOccupancy} {space.IsAccessible} ${space.DailyRate * daysNeeded} ");
                }
                else if (startMonth >= space.OpenFrom && startMonth <= space.OpenTo && endMonth >= space.OpenFrom && endMonth <= space.OpenTo && numGuests <= space.MaxOccupancy)
                {
                    matchSpaces.Add(space);
                    //Console.WriteLine($"{space.Id} {space.Name} ${space.DailyRate} {space.MaxOccupancy} {space.IsAccessible} ${space.DailyRate * daysNeeded} ");
                }

            }
            return matchSpaces;

        }

        public void GetSpaceInfoByVenue(int venueId)
        {
            Console.WriteLine();
        }




        //public DateTime ReserveASpaceTime()
        //{
        //    Console.Write("When do you need the space? Please use a MM/DD/YYYY format ");
        //    DateTime firstDay = DateTime.Parse(Console.ReadLine());

        //    return
        //}

        //public string ReserveASpaceDays()
        //{
        //    Console.Write("How many days will you need the space? ");
        //    return Console.ReadLine();
        //}

        //public string ReserveASpaceAttendees()
        //{
        //    Console.Write("How many people will be in attendance? ");
        //    return Console.ReadLine();
        //}

        //public void GetSpaceInfoByVenue(int venueId)
        //{
        //    Console.WriteLine();
        //}

        //string spaceTime = ReserveASpaceTime();
        //string spaceDays = ReserveASpaceDays();
        //string spaceAttendees = ReserveASpaceAttendees();
    }
}

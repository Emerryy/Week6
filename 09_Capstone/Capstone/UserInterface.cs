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
        private ReservationDAO resDAO;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
            spaceDAO = new SpacesDAO(connectionString);
            resDAO = new ReservationDAO(connectionString);
        }

        public void Run()
        {
            bool done = false;
            while (!done)
            {
                done = RunMainMenu();
            }
        }

        public bool RunMainMenu()
        {
            string result = PrintMainMenu();
            bool done = false;
            switch (result.ToLower())
            {
                case "1":
                    RunListVenuesMenu();
                    break;
                case "q":
                    Console.WriteLine("Thanks!");
                    done = true;
                    break;
                default:
                    Console.WriteLine("Please make a valid selection.");
                    break;
            }
            return done;
        }

        public string PrintMainMenu()
        {
            Console.WriteLine("Welcome to Excelsior Venues!");
            Console.WriteLine("1) List Venues");
            Console.WriteLine("Q) Quit");
            return Console.ReadLine();
        }

        public void RunListVenuesMenu()
        {
            bool done = false;
            while (!done)
            {
                PrintListVenuesMenu();
                string input = Console.ReadLine();
                int venueId;
                if (input.ToLower() == "r")
                {
                    done = true;
                }
                else if (!int.TryParse(input, out venueId) || !venueDAO.IsVenueIDValid(venueId))
                {
                    Console.Write("Please enter a valid venue ID, or R to go back: ");
                    input = Console.ReadLine();
                }
                else
                {
                    RunVenueDetailsMenu(venueId);
                    done = true;
                }
            }
        }

        public void PrintListVenuesMenu()
        {
            Console.WriteLine("Which venue would you like to view?");
            PrintVenues();
            Console.WriteLine("R) Return to Previous Screen");
            Console.WriteLine();
            Console.Write("Enter venue ID, or R to return to previous screen: ");
        }

        public void PrintVenues()
        {
            IList<Venue> venues = venueDAO.GetVenuesInAlphaOrder();
            foreach (Venue venue in venues)
            {
                Console.WriteLine($"{venue.Name}".PadRight(35) + $" Venue ID- {venue.ID}".PadRight(4));
            }
        }

        public bool RunVenueDetailsMenu(int venueId)
        {
            bool done = false;
            while (!done)
            {
                PrintVenueDetails(venueId);
                string input = PrintViewOrReserveChoices();
                switch (input.ToLower())
                {
                    //probably model, Loosely, from ViewOrReserveSpaceMenu
                    case "1":
                        RunViewSpacesMenu(venueId);
                        done = true;
                        break;
                    case "2":
                        RunMakeReservationMenu(venueId);
                        done = true;
                        break;
                    case "r":
                        done = true;
                        break;
                    default:
                        Console.Write("Please make a valid selection: ");
                        input = PrintViewOrReserveChoices();
                        break;
                }
            }
            return done;
        }

        public void PrintVenueDetails(int venueId) //todo better SQL for venues w/o categories
        {
            List<ListedVenue> venueRows = venueDAO.GetVenueInfoByID(venueId);
            ListedVenue venueWithCategories;
            if (venueRows.Count == 0)
            {
                venueWithCategories = venueRows[1];
            }
            else
            {
                venueWithCategories = venueRows[venueRows.Count - 1];
            }

            Console.WriteLine();
            Console.WriteLine($"{venueWithCategories.VenueName}");
            Console.WriteLine($"Location: {venueWithCategories.CityName}, {venueWithCategories.StateName}");
            Console.Write("Categories: ".PadRight(2));
            for (int i = 0; i < venueWithCategories.CategoryName.Count; i++)
            {
                if (i == venueWithCategories.CategoryName.Count - 1)
                {
                    Console.Write($"{venueWithCategories.CategoryName[i]}");
                    Console.WriteLine();
                }
                else
                {
                    Console.Write($"{venueWithCategories.CategoryName[i]}, ");
                }
            }
            Console.WriteLine();
            Console.WriteLine($"{venueWithCategories.VenueDescription}");
            Console.WriteLine();
        }

        public string PrintViewOrReserveChoices()
        {
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1) View Spaces");
            Console.WriteLine("2) Search for Reservation");
            Console.WriteLine("R) Return to Previous Screen");
            return Console.ReadLine();
        }

        public void RunViewSpacesMenu(int venueId)
        {
            string input = PrintSpaces(venueId);
            bool done = false;
            while (!done)
            {
                switch (input.ToLower())
                {
                    case "1":
                        RunMakeReservationMenu(venueId);
                        done = true;
                        break;
                    case "r":
                        done = true;
                        break;
                    default:
                        Console.WriteLine("Please make a valid selection.");
                        input = PrintSpaces(venueId);
                        break;
                }
            }
        }

        public void RunMakeReservationMenu(int venueId)
        {
            Reservation addedReservation = MakeReservation(venueId);
            if (addedReservation.SpaceId != -1)
            {
                PrintConfirmation(addedReservation);
            }
        }

        public void PrintConfirmation(Reservation addedReservation)
        {
            Console.WriteLine("Thanks for submitting your reservation! The details for your event are listed below:");
            Console.WriteLine();
            Console.WriteLine("Confirmation #: ".PadLeft(20) + addedReservation.ReservationId);
            Console.WriteLine("Venue: ".PadLeft(20) + addedReservation.VenueName);
            Console.WriteLine("Space: ".PadLeft(20) + addedReservation.SpaceName);
            Console.WriteLine("Reserved For: ".PadLeft(20) + addedReservation.ReservedFor);
            Console.WriteLine("Attendees: ".PadLeft(20) + addedReservation.NumberOfAttendees);
            Console.WriteLine("Arrival Date: ".PadLeft(20) + addedReservation.StartDate);
            Console.WriteLine("Depart Date: ".PadLeft(20) + addedReservation.EndDate);
            Console.WriteLine("Total Cost: ".PadLeft(20) + (addedReservation.DailyRate * addedReservation.NumberOfDaysReserved));
        }

        public Reservation MakeReservation(int venueId)
        {
            Reservation finalReservation = new Reservation();
            bool done = false;
            while (!done)
            {
                DateTime reserveDate = ReserveASpaceTime();
                int reserveDays = ReserveASpaceDays();
                int reserveGuests = ReserveASpaceAttendees();
                IList<Space> availableSpaces = ListAvailableSpaces(venueId, reserveDate, reserveDays, reserveGuests);
                int spaceId = GetSpaceIdForReservation();
                if (spaceId == 0 || !AreRequestedDaysAvailable(spaceId, reserveDate, reserveDays) || !spaceDAO.DoesSpaceHaveOccupancy(reserveGuests, spaceId))
                {
                    done = true;
                    finalReservation.SpaceId = -1;
                    Console.WriteLine("Reservation cancelled.");
                    Console.WriteLine();
                }
                else
                {
                    string resName = GetNameForReservation();
                    Reservation constructReservation = GatherReservationInfo(availableSpaces, spaceId, reserveDate, reserveDays, resName, reserveGuests);
                    finalReservation = resDAO.AddNewReservation(constructReservation);
                    done = true;
                }
            }
            return finalReservation;
        }



        public IList<Space> ListAvailableSpaces(int venueId, DateTime reserveDate, int reserveDays, int reserveGuests)
        {
            IList<Space> spaces = CheckAndReturnAvailSpaces(venueId, reserveDate, reserveDays, reserveGuests);
            Console.WriteLine();
            Console.WriteLine("These spaces are available based on your needs:");
            foreach (Space space in spaces)
            {
                Console.WriteLine($"{space.Id} {space.Name} ${space.DailyRate} {space.MaxOccupancy} {space.IsAccessible} ${space.DailyRate * reserveDays} ");
            }
            Console.WriteLine();
            return spaces;
        }

        public int GetSpaceIdForReservation()
        {
            bool done = false;
            int spaceId = 0;
            while (!done)
            {
                Console.WriteLine("Which space would you like to reserve? Enter 0 to cancel. ");
                string input = Console.ReadLine();
                bool succeeded = int.TryParse(input, out spaceId);
                if (spaceId == 0)
                {
                    done = true;
                }
                else if (!succeeded || !spaceDAO.IsSpaceIDValid(spaceId))
                {
                    Console.WriteLine("Please enter a valid space ID#.");
                }
                else
                {
                    done = true;
                }
            }
            return spaceId;
        }

        //        bool done = false;
        //            while (!done)
        //            {
        //                PrintListVenuesMenu();
        //        string input = Console.ReadLine();
        //        int venueId;
        //                if (input.ToLower() == "r")
        //                {
        //                    done = true;
        //                }
        //                else if (!int.TryParse(input, out venueId) || !venueDAO.IsVenueIDValid(venueId))
        //                {
        //                    Console.Write("Please enter a valid venue ID, or R to go back: ");
        //                    input = Console.ReadLine();
        //                }
        //                else
        //{
        //    RunVenueDetailsMenu(venueId);
        //    done = true;
        //}
        //            }


        public string GetNameForReservation()
        {
            Console.WriteLine("Who is this reservation for?");
            return Console.ReadLine();
        }

        public Reservation GatherReservationInfo(IList<Space> spaces, int spaceId, DateTime reserveDate, int reserveDays, string resName, int reserveGuests)
        {
            Reservation buildingRes = new Reservation();
            buildingRes.ReservationId = 0;
            buildingRes.SpaceId = spaceId;
            buildingRes.NumberOfAttendees = reserveGuests;
            buildingRes.StartDate = reserveDate;
            buildingRes.EndDate = reserveDate.AddDays(reserveDays);
            buildingRes.NumberOfDaysReserved = reserveDays;
            buildingRes.ReservedFor = resName;

            return buildingRes;
        }

        public string PrintSpaces(int venueId)
        {
            IList<Space> spacesList = spaceDAO.GetSpacesInfoFromVenueId(venueId);

            Console.WriteLine();
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
            Console.WriteLine();
            Console.Write("When do you need the space? Please use a MM/DD/YYYY format ");
            DateTime firstDay = DateTime.Parse(Console.ReadLine());

            return firstDay;
        }

        public int ReserveASpaceDays()
        {
            int numberOfReserveDays = 0;
            bool done = false;
            while (!done)
            {
                Console.Write("How many days will you need the space? ");
                string input = Console.ReadLine();
                bool succeeded = int.TryParse(input, out numberOfReserveDays);
                if (!succeeded)
                {
                    Console.WriteLine("Please enter an integer number.");
                }
                else
                {
                    done = true;
                }
            }
            return numberOfReserveDays;
        }

        public int ReserveASpaceAttendees()
        {
            int numberOfAttendees = 0;
            bool done = false;
            while (!done)
            {
                Console.Write("How many people will be in attendance? ");
                string input = Console.ReadLine();
                bool succeeded = int.TryParse(input, out numberOfAttendees);
                if (!succeeded)
                {
                    Console.WriteLine("Please enter an integer number.");
                }
                else
                {
                    done = true;
                }
            }
            return numberOfAttendees;
        }

        public IList<Space> CheckAndReturnAvailSpaces(int venueId, DateTime reserveDate, int reserveDays, int reserveGuests)
        {
            IList<Space> spacesList = spaceDAO.GetTop5SpacesInfoFromVenueId(venueId);
            DateTime endDate = reserveDate.AddDays(reserveDays);
            IList<Space> matchSpaces = new List<Space>();

            int endMonth = endDate.Month;
            int startMonth = reserveDate.Month;


            foreach (Space space in spacesList)
            {
                if (space.OpenFrom == -1 && reserveGuests <= space.MaxOccupancy && AreRequestedDaysAvailable(space.Id, reserveDate, reserveDays))
                {
                    matchSpaces.Add(space);
                }
                else if (startMonth >= space.OpenFrom && startMonth <= space.OpenTo && endMonth >= space.OpenFrom && endMonth <= space.OpenTo && reserveGuests <= space.MaxOccupancy && AreRequestedDaysAvailable(space.Id, reserveDate, reserveDays))
                {
                    matchSpaces.Add(space);
                }
            }
            return matchSpaces;
        }

        public bool AreRequestedDaysAvailable(int spaceId, DateTime reserveDate, int reserveDays)
        {
            bool success = false;
            DateTime lastDate = reserveDate.AddDays(reserveDays);
            List<DateTime> datesBooked = resDAO.GetBookedDates(spaceId);

            foreach (DateTime dateTime in datesBooked)
            {
                if (!datesBooked.Contains(reserveDate) && !datesBooked.Contains(lastDate))
                {
                    success = true;
                }
            }
            return success;
        } 
    }
}

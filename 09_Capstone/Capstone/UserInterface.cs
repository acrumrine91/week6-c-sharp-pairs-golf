using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;

namespace Capstone
{
    public class UserInterface
    {
        //ALL Console.ReadLine and WriteLine in this class
        //NONE in any other class

        public string connectionString;
        private IVenueDAO venueDAO;
        private ICityDAO cityDAO;
        private ISpaceDAO spaceDAO;
        private IReservationDAO reservationDAO;


        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueSqlDAO(connectionString);
            cityDAO = new CitySqlDAO(connectionString);
            spaceDAO = new SpaceSqlDAO(connectionString);
            reservationDAO = new ReservationSqlDAO(connectionString);

        }

        //public UserInterface(IVenueDAO venueDAO)
        //{
        //    this.venueDAO = venueDAO;
        //}


        public void Run()
        {

            bool done = false;
            while (!done)
            {
                Console.WriteLine("Welcome to Excelsior Venue Services.");
                Console.WriteLine("You can use this system to seek and book our available spaces.");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("~==MAIN MENU==~");
                Console.WriteLine("Please select from the menu below.");
                Console.WriteLine("");
                Console.WriteLine("1) List Venues");
                Console.WriteLine("Q) or any other key to quit");
                string userInput = Console.ReadLine();

                if (userInput == "1")
                {
                    GetVenues();
                }
                else done = true;
            }
        }

        private void GetVenues()
        {
            bool done = false;
            while (!done)
            {
                IList<Venue> venues = venueDAO.GetVenues();
                Console.WriteLine("~~ View Venues Menu ~~");
                Console.WriteLine("Please type in the number of the venue you want to select below");
                for (int index = 0; index < venues.Count; index++)
                {
                    Console.WriteLine(index + 1 + ") - " + venues[index].Name);
                }
                Console.WriteLine("Press any other key to RETURN to our main menu.");
                string userInput = Console.ReadLine();

                bool isParsable = Int32.TryParse(userInput, out int userNumber);
                int venueNum = userNumber - 1;
                if (isParsable)
                {
                    if ((venueNum >= 0) && (venueNum < venues.Count))
                    {
                        VenueDetailsMenu(venueNum);
                    }
                    else return;
                }
                else done = true;
            }

        }

        public void VenueDetailsMenu(int venueNum)
        {
            bool done = false;
            while (!done)
            {
                IList<Venue> venues = venueDAO.GetVenues();
                City city = cityDAO.GetVenueCity(venues[venueNum].CityId);
                //Need to get city and state lists and then write their info below
                //same ways we did the other method
                Console.WriteLine("");
                Console.WriteLine("~~VENUE DETAILS~~");
                Console.WriteLine("");
                Console.WriteLine(venues[venueNum].Name);
                Console.WriteLine("Location: " + city.Name + ", " + city.StateAbbreviation);
                Console.WriteLine("");
                Console.WriteLine(venues[venueNum].Description);
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("What would you like to do next?");
                Console.WriteLine("");
                Console.WriteLine("1) View Spaces");
                Console.WriteLine("2) Search for reservation");
                Console.WriteLine("");
                Console.WriteLine("Press any other key to RETURN to our list of Venues");
                string userInput = Console.ReadLine();

                if (userInput == "1")
                {
                    ListVenueSpacesMenu(venueNum);
                }
                else if (userInput == "2")
                {
                    SearchAndMakeReservationMenu(venueNum);
                }

                else done = true;
            }
        }

        public void ListVenueSpacesMenu(int venueNum)
        {
            venueNum += 1;
            bool done = false;
            while (!done)
            {
                IList<Venue> venues = venueDAO.GetVenues();
                IList<Space> spaces = spaceDAO.GetVenueSpaces(venueNum);
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine(venues[venueNum].Name);
                Console.WriteLine("");
                Console.WriteLine("ID".PadRight(4) + "Name".PadRight(25) + "Handicap Access".PadRight(20) +
                    "Open".PadRight(10) + "Close".PadRight(10) + "Daily Rate".PadRight(15) + "Max Occup.".PadRight(10));
                foreach (Space space in spaces)
                {
                    Console.WriteLine(space);
                }
                Console.WriteLine("");
                Console.WriteLine("What would you like to do next?");
                Console.WriteLine("1) Reserve a space");
                Console.WriteLine("Press any other key to RETURN to the venue details");
                string userInput = Console.ReadLine();
                if (userInput == "1")
                {
                    SearchAndMakeReservationMenu(venueNum);
                }
                done = true;
            }

        }

        public void SearchAndMakeReservationMenu(int venueNum)
        {
            IList<Venue> venues = venueDAO.GetVenues();
            IList<Space> spaces = spaceDAO.GetVenueSpaces(venueNum);
            IList<Reservation> reservations = reservationDAO.GetReservations(spaces);
            bool done = false;
            while (!done)
            {
                Console.WriteLine("Search for available spaces");
                Console.WriteLine("When do you need the space? MM/DD/YEAR");
                string inputDay = Console.ReadLine();

                bool correctInput = DateTime.TryParse(inputDay, out DateTime startDate);
                if (correctInput == false || startDate < DateTime.Now)
                {
                    Console.WriteLine("Please input a correct date format");
                    return;
                }
                Console.WriteLine("How many days will you need the space?");
                string dayNumber = Console.ReadLine();

                bool parseDays = Int32.TryParse(dayNumber, out int numOfDays);
                if (parseDays == false || numOfDays <= 0)
                {
                    Console.WriteLine("Please input a number of days");
                    return;
                }
                Console.WriteLine("How many people will be in attendance?");
                string attendNum = Console.ReadLine();

                bool parseOccupancy = Int32.TryParse(dayNumber, out int peopleAttending);
                if (parseOccupancy == false || peopleAttending <= 0)
                {
                    Console.WriteLine("Please input the number of people attending");
                    return;
                }
                //bool isAvailable = reservationDAO.AreDatesAvailable(spaces, reservations, startDate, numOfDays);
                //Console.WriteLine("Still testing");

                List<Space> toRemove = new List<Space>();
                foreach (Space space in spaces)
                {
                    bool available = reservationDAO.IsDateAvailable(reservations, space, startDate, numOfDays);
                    if (available == false)
                    {
                        toRemove.Add(space);
                    }
                }
                foreach (Space space in spaces)
                {
                    bool available = reservationDAO.IsSpaceOperating(space, startDate, numOfDays);
                    if (available == false)
                    {
                        toRemove.Add(space);
                    }
                }
                foreach (Space space in spaces)
                {
                    bool available = reservationDAO.IsBookingBelowMaxOcc(space, peopleAttending);
                    if (available == false)
                    {
                        toRemove.Add(space);
                    }
                }
                foreach (Space removal in toRemove)
                {
                    spaces.Remove(removal);
                }

                Console.WriteLine("");
                Console.WriteLine("Space #".PadRight(9) + "Name".PadRight(25) + "Daily Rate".PadRight(12) +
                "Max Occup".PadRight(10) + "Accessible?".PadRight(12) + "Total Cost".PadRight(13));
                foreach (Space space in spaces)
                {
                    decimal totalCost = space.DailyRate * numOfDays;
                    Console.WriteLine(space.Id.ToString().PadRight(9) + space.Name.PadRight(25) +
                        space.DailyRate.ToString("C").PadRight(12) + space.MaxOccupancy.ToString().PadRight(10) +
                        space.IsAccessible.ToString().PadRight(12) + totalCost.ToString("C"));

                }
                Console.WriteLine("");
                Console.WriteLine("Which space would you like to reserve? (Please enter Space #)");
                string spaceIDChosen = Console.ReadLine();
                Console.WriteLine("Who is this reservation for?");
                string reservedFor = Console.ReadLine();

                //ADD METHOD TO GO TO CONFIRMATION AND TO ADD TO RESERVATION DATABASE
                PrintReservationConfirmation(spaceIDChosen, reservedFor, startDate, numOfDays, peopleAttending);


            }
        }
        public void PrintReservationConfirmation(string spaceIDChosen, string reservedFor, DateTime startDate, int numOfDays, int peopleAttending)
        {
            Space bookedSpace = spaceDAO.GetBookedSpaceDetails(spaceIDChosen);
            string confirmationID = reservationDAO.AddReservationToSql(spaceIDChosen, reservedFor, startDate, numOfDays, peopleAttending);


        }

    }

}


using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;
using System.Globalization;

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


            try
            {
                //requires a try catch around it because we parse inside a while loop
                bool done = false;
                while (!done)
                {
                    IList<Venue> venues = venueDAO.GetVenues();
                    Console.WriteLine("~~ View Venues Menu ~~");
                    Console.WriteLine("Please type in the number of the venue you want to select below");
                    for (int index = 0; index < venues.Count; index++) //if they hit any number other than a venue number, goes back a menu
                    {
                        Console.WriteLine(index + 1 + ") - " + venues[index].Name);
                    }
                    Console.WriteLine("Press any other key to RETURN to our main menu.");
                    string userInput = Console.ReadLine();

                    int inputNumber = int.Parse(userInput);
                    int venueIndexNum = inputNumber - 1;

                    if ((venueIndexNum >= 0) && (venueIndexNum < venues.Count))
                    {
                        VenueDetailsMenu(venueIndexNum);
                    }


                    else done = true;
                }

            }
            catch (System.FormatException)
            {
                Console.WriteLine("Please enter a valid venue number");
                return;
            }
        }

        public void VenueDetailsMenu(int venueIndexNum)
        {
            bool done = false;
            while (!done)
            {
                IList<Venue> venues = venueDAO.GetVenues();
                City city = cityDAO.GetVenueCity(venues[venueIndexNum].CityId);
                //Need to get city and state lists and then write their info below
                //same ways we did the other method
                Console.WriteLine("");
                Console.WriteLine("~~VENUE DETAILS~~");
                Console.WriteLine("");
                Console.WriteLine(venues[venueIndexNum].Name);
                Console.WriteLine("Location: " + city.Name + ", " + city.StateAbbreviation);
                Console.WriteLine("");
                Console.WriteLine(venues[venueIndexNum].Description);
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
                    ListVenueSpacesMenu(venueIndexNum);
                }
                else if (userInput == "2")
                {
                    SearchAndMakeReservationMenu(venueIndexNum);

                }

                else done = true;
            }
        }

        public void ListVenueSpacesMenu(int venueNum)
        {
            venueNum += 1; //switches from checking against the index to checking against the database ID
            bool done = false;
            while (!done)
            {
                IList<Venue> venues = venueDAO.GetVenues();
                IList<Space> spaces = spaceDAO.GetVenueSpaces(venueNum);
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine(venues[venueNum - 1].Name);
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
                    venueNum -= 1; // has to switch back to checking against index because of the search/make res menu +1
                    SearchAndMakeReservationMenu(venueNum);
                }
                done = true;
            }

        }

        public void SearchAndMakeReservationMenu(int venueNum)
        {
            venueNum += 1;
            IList<Venue> venues = venueDAO.GetVenues();
            IList<Space> spaces = spaceDAO.GetVenueSpaces(venueNum);
            IList<Reservation> reservations = reservationDAO.GetReservations(spaces);
            DateTime startDate = new DateTime();
            int numOfDays = 0;
            int peopleAttending = 0;

            bool done = false;
            while (!done)
            {
                //different try catch statements for each parse check because a different error message is returned
                try
                {
                    Console.WriteLine("Search for available spaces");
                    Console.WriteLine("When do you need the space? MM/DD/YEAR");
                    string inputDay = Console.ReadLine();

                    startDate = DateTime.Parse(inputDay);
                    if (startDate < DateTime.Now)
                    {
                        Console.WriteLine("Please input a future date!");
                        return;
                    }
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Please enter your date as a Month Number/Day/Full Year");
                    return;
                }

                Console.WriteLine("How many days will you need the space?");
                string dayNumber = Console.ReadLine();
                try
                {
                    numOfDays = Convert.ToInt32(dayNumber);
                    if (numOfDays <= 0)
                    {
                        Console.WriteLine("Please input a positive number of days");
                        return;
                    }
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Please enter the length of your reservation as a number");
                    return;
                }

                Console.WriteLine("How many people will be in attendance?");
                string attendNum = Console.ReadLine();

                try
                {
                    peopleAttending = Convert.ToInt32(attendNum);
                    if (peopleAttending <= 0)
                    {
                        Console.WriteLine("Please input the number of people attending");
                        return;
                    }
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Please enter the expected attendance as a number");
                    return;
                }

                List<Space> toRemove = new List<Space>();
                //creates a list of items to remove from the list of Spaces - each bool check adds to the list
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
                //removes all Space objects that are in the list toRemove from the list spaces

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
                PrintReservationConfirmation(venueNum, spaceIDChosen, reservedFor, startDate, numOfDays, peopleAttending);

                done = true;
            }
        }
        public void PrintReservationConfirmation(int venueNum, string spaceIDChosen, string reservedFor, DateTime startDate, int numOfDays, int peopleAttending)
        {

            Space bookedSpace = spaceDAO.GetBookedSpaceDetails(spaceIDChosen);
            IList<Venue> venues = venueDAO.GetVenues();
            string confirmationID = reservationDAO.AddReservationToSql(spaceIDChosen, reservedFor, startDate, numOfDays, peopleAttending);
            string numattend = Convert.ToString(peopleAttending);
            
            string startDateString = Convert.ToString(startDate);
            DateTime endDate = startDate.AddDays(numOfDays);
            string endDateString = Convert.ToString(endDate);
            
            string totalCost = Convert.ToString(numOfDays * bookedSpace.DailyRate);
            totalCost.Substring(totalCost.Length - 2);


            Console.WriteLine("");
            Console.WriteLine("Thanks for submitting your reservation!");
            Console.WriteLine("Your details and confirmation ID are listed below");
            Console.WriteLine("");
            Console.WriteLine("Confirmation #: " + confirmationID);
            Console.WriteLine("Venue: " + venues[venueNum].Name);
            Console.WriteLine("Space: " + bookedSpace.Name);
            Console.WriteLine("Reserved For: " + reservedFor);
            Console.WriteLine("Attendees: " + numattend);
            Console.WriteLine("Arrival Date: " + startDateString);
            Console.WriteLine("Depart Date: " + endDateString);
            Console.WriteLine("Total Cost: $" + totalCost);
            Console.WriteLine("");
            Console.WriteLine("Press any key and/or ENTER to leave the confirmation print scren and view a list of venues");
            string end = Console.ReadLine();
            GetVenues();

        }

    }

}


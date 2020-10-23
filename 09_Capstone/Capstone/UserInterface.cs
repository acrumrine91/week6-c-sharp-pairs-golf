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


        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueSqlDAO(connectionString);
            cityDAO = new CitySqlDAO(connectionString);
            spaceDAO = new SpaceSqlDAO(connectionString);

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
                // is search for reservation bonus???
                Console.WriteLine("");
                Console.WriteLine("Press any other key to RETURN to our list of Venues");
                string userInput = Console.ReadLine();

                if (userInput == "1")
                {
                    ListVenueSpacesMenu(venueNum);
                }

                else done = true;
            }
        }

        public void ListVenueSpacesMenu(int venueNum)
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
            Console.ReadLine();

        }

    }
}

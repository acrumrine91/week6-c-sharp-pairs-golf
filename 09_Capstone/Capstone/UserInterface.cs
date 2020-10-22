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


        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueSqlDAO(connectionString);

        }

        //public UserInterface(IVenueDAO venueDAO)
        //{
        //    this.venueDAO = venueDAO;
        //}


        public void Run()
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
            else return;
        }

        private void GetVenues()
        {
            IList<Venue> venues = venueDAO.GetVenues();
            Console.WriteLine("This is the view venues menu");
            for (int index = 0; index < venues.Count; index++)
            {
                Console.WriteLine(index + 1 + ") - " + venues[index].Name);

               // Console.WriteLine(Venue.Name);
            }
            
            Console.WriteLine("R) or press any other key to return to main menu");
            string userInput = Console.ReadLine();

            
          
        }

        public void VenuDetailsMenu(string userInput)
        {
            //GetVenueDetails(); Method to call details listed below


            //Console.WriteLine("name of venue"); // use connection string to use name selected
            //Console.WriteLine("Location: (city name), (city state abbrev)"); // use city and city state associated with venue selected
            //Console.WriteLine("Categories: "); //(categories associated with venue selected)
            //Console.WriteLine("");
            //Console.WriteLine("venue description"); // use description associated with the venue
            //Console.WriteLine("");
            //Console.WriteLine("1) View Venue Spaces");
            //Console.WriteLine("2) Search for Reservation"); (might BE A BONE US)
            //Console.WriteLine("R) or press any other key to return to the list of venues");
            //string nextInput = Console.ReadLine();

        }

        public void ListVenueSpacesMenu()
        {
            Console.WriteLine("Name of venue space they selected");

            //list of spaces with info goes here


            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1) Reserve a space");

        }

    }
}

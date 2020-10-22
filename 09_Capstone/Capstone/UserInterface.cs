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

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Run()
        {
            Console.WriteLine("Reached the User Interface.");
            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;

class Program
{



    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    static void Main(string[] args)
    {
        UserManager.UserAdd userAdd = new UserManager.UserAdd();

        while (true)
        {
            Console.Write("Enter Employer Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Employer Password: ");
            string password = Console.ReadLine();
            userAdd.AddEmployer(username, password);
        }
    }
}

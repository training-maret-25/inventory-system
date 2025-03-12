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

    public class UserAdd
    {
        private List<User> users = new List<User>();
        private int nextId = 1;

        public void AddEmployer(string username, string password)
        {
            foreach (var user in users)
            {
                if (user.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("ussername udh ada.");
                    return;
                }
            }

            User newUser = new User
            {
                Id = nextId,
                Username = username,
                Password = password,
                Role = "Employer"
            };

            users.Add(newUser);
            nextId++;

            Console.WriteLine($"sudah berhasil.");
        }
    }

    static void Main(string[] args)
    {
        UserAdd userAdd = new UserAdd();

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


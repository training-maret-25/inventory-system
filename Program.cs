using System;
<<<<<<< HEAD
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
=======

class Program
{
    static void Main()
    {
        UserManager userManager = new UserManager();

        Console.Write("Masukkan username: ");
        string username = Console.ReadLine();

        Console.Write("Masukkan password: ");
        string password = Console.ReadLine();

        
        if (userManager.Login(username, password))
        {
            Console.WriteLine("Login sukses!");

            Console.WriteLine("Tekan ENTER untuk logout....");
            Console.ReadLine();
            userManager.Logout();
        }
        else
        {
            Console.WriteLine("Login gagal.");
        }
    }
}

>>>>>>> 350007a59da5d68b2291cb6080e8f2dc25a15f56

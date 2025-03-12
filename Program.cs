using System;
<<<<<<< HEAD

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
=======
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
                    Console.WriteLine("Username udh ada");
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

            Console.WriteLine("Berhasil Menambahkan");
        }
    }
    static void Main(string[] args)
    {
        UserAdd userAdd = new UserAdd();
        while (true)
        {
            Console.Write("Masukkan nama Employer ");
            string username = Console.ReadLine();
            Console.Write("Masukkan password Employer ");
            string password = Console.ReadLine();
            userAdd.AddEmployer(username, password);
        }
    }
}
>>>>>>> ae8a58cb8d319bdcc1994a2c422aba0ccfea0fd1

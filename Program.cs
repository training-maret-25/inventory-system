﻿using System;
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

<<<<<<< HEAD
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
=======
        Console.Write("Masukkan username: ");
        string username = Console.ReadLine() ?? "";

        Console.Write("Masukkan password: ");
        string password = Console.ReadLine() ?? "";


        bool isSuccess = userManager.Login(username, password);

        if (!isSuccess)
        {
            Console.WriteLine("Login gagal.");
>>>>>>> 4d356704247bda9272a90665a90689dbf0879f0b
        }
        else
        {
            Console.WriteLine("Login sukses.");
        }

        Console.WriteLine("Tekan tombol apapun untuk keluar...");
        Console.ReadKey();
    }
}

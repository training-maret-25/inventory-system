﻿using System;
using System.Collections.Generic;

namespace InventorySystem
{

    static class Program
    {
        static void Main()
        {
            HashConverter.ConvertPasswords();

            UserManager userManager = new UserManager();

            Console.Write("Masukkan username Admin: ");
            string adminUsername = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Masukkan password Admin: ");
            string adminPassword = Console.ReadLine()?.Trim() ?? "";

            User? adminUser = userManager.Authenticate(adminUsername, adminPassword);

            if (adminUser == null || adminUser.Role != "Admin")
            {
                Console.WriteLine("Login gagal! Hanya Admin yang bisa mengedit user.");
                return;
            }

            Console.Write("Masukkan ID user yang ingin diedit: ");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.Write("Masukkan username baru (biarkan kosong jika tidak ingin mengubah): ");
                string newUsername = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Masukkan password baru (biarkan kosong jika tidak ingin mengubah): ");
                string newPassword = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Masukkan role baru (Admin/Employee, kosong jika tidak ingin mengubah): ");
                string newRole = Console.ReadLine()?.Trim() ?? "";

                // Validasi role hanya jika diisi
                if (!string.IsNullOrEmpty(newRole) && newRole != "Admin" && newRole != "Employee")
                {
                    Console.WriteLine("Role tidak valid! Gunakan 'Admin' atau 'Employee'.");
                    return;
                }

                userManager.EditUser(adminUser.Id, userId, newUsername, newPassword, newRole);
            }
            else
            {
                Console.WriteLine("ID tidak valid!");
            }




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

        Console.Write("Masukkan username: ");
        string username = Console.ReadLine() ?? "";

        Console.Write("Masukkan password: ");
        string password = Console.ReadLine() ?? "";


        bool isSuccess = userManager.Login(username, password);

        if (!isSuccess)
        {
            Console.WriteLine("Login gagal.");
        }
        else
        {
            Console.WriteLine("Login sukses.");
        }

        Console.WriteLine("Tekan tombol apapun untuk keluar...");
        Console.ReadKey();
    }

}

}

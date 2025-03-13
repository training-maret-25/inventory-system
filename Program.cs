using System;
using InventorySystem.Services;

namespace InventorySystem
{
    class Program
    {
        static void Main()
        {
            UserManager userManager = new UserManager();
            bool isRunning = true;

            Console.WriteLine("=== Aplikasi Manajemen User ===\n");

            while (isRunning)
            {
                Console.WriteLine("\nMenu Utama:");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Tambah User");
                Console.WriteLine("3. Tampilkan Daftar User");
                Console.WriteLine("4. Hapus User (Admin Only)");
                Console.WriteLine("5. Logout");
                Console.WriteLine("6. Keluar Aplikasi");

                Console.Write("\nPilih menu (1-6): ");
                string? pilihan = Console.ReadLine();

                switch (pilihan)
                {
                    case "1":
                        Console.Write("Masukkan username: ");
                        string username = Console.ReadLine() ?? "";

                        Console.Write("Masukkan password: ");
                        string password = Console.ReadLine() ?? "";

                        userManager.Login(username, password);
                        break;

                    case "2":
                        userManager.AddUser();
                        break;

                    case "3":
                        userManager.DisplayUsers();
                        break;

                    case "4":
                        Console.Write("Masukkan ID User yang ingin dihapus: ");
                        string? inputId = Console.ReadLine();

                        if (int.TryParse(inputId, out int userId))
                        {
                            userManager.DeleteUserById(userId);
                        }
                        else
                        {
                            Console.WriteLine("❌ ID tidak valid. Harus berupa angka.");
                        }
                        break;

                    case "5":
                        userManager.Logout();
                        break;

                    case "6":
                        Console.WriteLine("👋 Keluar dari aplikasi. Sampai jumpa!");
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("❌ Pilihan tidak valid. Silakan pilih 1-6.");
                        break;
                }
            }
        }
    }
}

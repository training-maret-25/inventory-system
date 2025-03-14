using System;
using InventorySystem.Services;

namespace InventorySystem
{
    class Program
    {
        static void Main()
        {
            var userManager = new UserManager();
            var inventoryManager = new InventoryManager();

            string role = ""; // Pastikan variabel role diinisialisasi
            int userId = 0; // Pastikan variabel userId diinisialisasi
            bool isLoggedIn = false; 

            Console.Clear();
            Console.WriteLine("=== SELAMAT DATANG DI SISTEM INVENTORY ===");

            // LOGIN SYSTEM
            while (!isLoggedIn)
            {
                Console.Write("\nUsername: ");
                string username = Console.ReadLine() ?? "";

                Console.Write("Password: ");
                string password = Console.ReadLine() ?? "";

                isLoggedIn = userManager.Login(username, password, out role, out userId);

                if (!isLoggedIn)
                {
                    Console.WriteLine("Login gagal. Coba lagi.");
                }
            }
            
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== MENU UTAMA ===");
                Console.WriteLine("1. Lihat Daftar Barang");
                Console.WriteLine("2. Tambah Barang");
                Console.WriteLine("3. Edit Barang");
                Console.WriteLine("4. Hapus Barang");
                Console.WriteLine("5. Cek Barang Perlu Restok");

                if (role == "admin")
                {
                    Console.WriteLine("6. Manajemen User");
                }
                else if (role == "employee")
                {
                    Console.WriteLine("6. Edit Akun Saya");
                }

                Console.WriteLine("0. Logout & Keluar");
                Console.Write("\nPilih menu: ");
                string pilihan = Console.ReadLine() ?? "";

                switch (pilihan)
                {
                    case "1":
                        inventoryManager.ListItems();
                        break;
                    case "2":
                        inventoryManager.AddItem();
                        break;
                    case "3":
                        Console.Write("Masukkan ID Barang yang ingin di-edit: ");
                        if (int.TryParse(Console.ReadLine(), out int editId))
                        {
                            inventoryManager.EditItem(editId);
                        }
                        else
                        {
                            Console.WriteLine("ID tidak valid.");
                        }
                        break;
                    case "4":
                        Console.Write("Masukkan ID Barang yang ingin dihapus: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                        {
                            inventoryManager.DeleteItem(deleteId);
                        }
                        else
                        {
                            Console.WriteLine("ID tidak valid.");
                        }
                        break;
                    case "5":
                        inventoryManager.CheckRestockItems();
                        break;
                    case "6":
                        if (role == "admin") UserManagementMenu(userManager);
                        else if (role == "employee") EditUser(userManager, userId);
                        break;
                    case "0":
                        userManager.Logout();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }
            }

            Console.WriteLine("Terima kasih telah menggunakan sistem ini.");
        }

        static void UserManagementMenu(UserManager userManager)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n=== MENU MANAJEMEN USER ===");
                Console.WriteLine("1. Lihat Daftar User");
                Console.WriteLine("2. Tambah User");
                Console.WriteLine("3. Edit User");
                Console.WriteLine("4. Hapus User");
                Console.WriteLine("0. Kembali");

                Console.Write("\nPilih menu: ");
                string pilihan = Console.ReadLine() ?? "";

                switch (pilihan)
                {
                    case "1":
                        userManager.DisplayUsers();
                        break;
                    case "2":
                        userManager.AddUser();
                        break;
                    case "3":
                        Console.Write("Masukkan ID User yang ingin di-edit: ");
                        if (int.TryParse(Console.ReadLine(), out int userId))
                        {
                            EditUser(userManager, userId);
                        }
                        else
                        {
                            Console.WriteLine("ID tidak valid.");
                        }
                        break;
                    case "4":
                        Console.Write("Masukkan ID User yang ingin dihapus: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            userManager.DeleteUserById(id);
                        }
                        else
                        {
                            Console.WriteLine("ID tidak valid.");
                        }
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }
            }
        }

        static void EditUser(UserManager userManager, int userId)
        {
            Console.Write("Masukkan username baru (kosongkan jika tidak ingin mengubah): ");
            string? newUsername = Console.ReadLine();
            Console.Write("Masukkan password baru (kosongkan jika tidak ingin mengubah): ");
            string? newPassword = Console.ReadLine();
            userManager.EditUser(userId, userId, newUsername, newPassword, null);
        }
    }
}

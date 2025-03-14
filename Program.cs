using System;
using InventorySystem.Services;
using logger;

namespace InventorySystem
{
    class Program
    {
        static void Main()
        {
            UserManager userManager = new UserManager();
            string role;

            Console.WriteLine("=== Sistem Manajemen User ===");
            Console.Write("Masukkan username: ");
            string username = Console.ReadLine() ?? "";
            Console.Write("Masukkan password: ");
            string password = Console.ReadLine() ?? "";

            if (!userManager.Login(username, password, out role))
            {
                Console.WriteLine("❌ Login gagal! Program berakhir.");
                return;
            }

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n=== Menu ===");
                Console.WriteLine("1. Lihat daftar user");
                Console.WriteLine("2. Edit username/password");
                if (role == "Admin")
                {
                    Console.WriteLine("3. Tambah user baru");
                    Console.WriteLine("4. Hapus user");
                }
                Console.WriteLine("5. Logout");
                Console.Write("Pilih menu: ");

                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        userManager.DisplayUsers();
                        break;
                    case "2":
                        EditUser(userManager, username, role);
                        break;
                    case "3":
                        if (role == "Admin") userManager.AddUser();
                        else Console.WriteLine("❌ Akses ditolak!");
                        break;
                    case "4":
                        if (role == "Admin") DeleteUser(userManager);
                        else Console.WriteLine("❌ Akses ditolak!");
                        break;
                    case "5":
                        userManager.Logout();
                        running = false;
                        break;
                    default:
                        Console.WriteLine("❌ Pilihan tidak valid!");
                        break;
                }
            }
        }

        static void EditUser(UserManager userManager, string username, string role)
        {
            Console.Write("Masukkan ID user yang ingin diedit: ");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("❌ ID tidak valid!");
                return;
            }

            Console.Write("Masukkan username baru (kosong jika tidak ingin mengubah): ");
            string newUsername = Console.ReadLine() ?? "";

            Console.Write("Masukkan password baru (kosong jika tidak ingin mengubah): ");
            string newPassword = Console.ReadLine() ?? "";

            string newRole = "";
            if (role == "Admin")
            {
                Console.Write("Masukkan role baru (Admin/Employee) atau kosong jika tidak ingin mengubah: ");
                newRole = Console.ReadLine() ?? "";
            }

            if (userManager.EditUser(userId, userId, newUsername, newPassword, newRole))
                Console.WriteLine("✅ User berhasil diperbarui!");
                
            else
                Console.WriteLine("❌ Gagal memperbarui user!");
        }

        static void DeleteUser(UserManager userManager)
        {
            Console.Write("Masukkan ID user yang ingin dihapus: ");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("❌ ID tidak valid!");
                return;
            }

            userManager.DeleteUserById(userId);
        }
    }
}

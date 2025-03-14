using System;
using InventorySystem.Services;

namespace InventorySystem
{
    class Program
    {
        static void Main()
        {
            UserManager userManager = new UserManager();
            string role;
            int userId; // Tambahkan variabel userId di sini


            Console.WriteLine("=== Sistem Manajemen User ===");
            Console.Write("Masukkan username: ");
            string username = Console.ReadLine() ?? "";
            Console.Write("Masukkan password: ");
            string password = Console.ReadLine() ?? "";

            if (!userManager.Login(username, password, out role, out userId))
            {
                Console.WriteLine("❌ Login gagal! Program berakhir.");
                return;
            }

            bool running = true;
            while (running)
{
    Console.WriteLine("\n=== Menu ===");
    Console.WriteLine("edit    - Edit username/password");
    if (role == "admin")
    {
        Console.WriteLine("lihat   - Lihat daftar user");
        Console.WriteLine("tambah  - Tambah user baru");
        Console.WriteLine("hapus   - Hapus user");
    }
    Console.WriteLine("logout  - Logout");
    Console.Write("Pilih menu: ");

    // Membaca input dan memastikan tidak terpengaruh huruf besar/kecil serta spasi berlebih
    string choice = Console.ReadLine()?.Trim().ToLower() ?? "";

    switch (choice)
    {
        case "lihat":
            if (role == "admin") userManager.DisplayUsers();
            else Console.WriteLine("❌ Akses ditolak!");
            break;
        case "edit":
         EditUser(userManager, username, role);
            break;
        case "tambah":
            if (role == "admin") userManager.AddUser();
            else Console.WriteLine("❌ Akses ditolak!");
            break;
        case "hapus":
            if (role == "admin") DeleteUser(userManager);
            else Console.WriteLine("❌ Akses ditolak!");
            break;
        case "logout":
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

using InventorySystem.Services;
using System;

namespace InventorySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var userManager = new UserManager();
            var inventoryManager = new InventoryManager();
            var transactionManager = new TransactionManager();

            string role = "";
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

                isLoggedIn = userManager.Login(username, password, out role);

                if (!isLoggedIn)
                {
                    Console.WriteLine("❌ Login gagal. Coba lagi.");
                }
            }

            // MENU UTAMA
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== MENU UTAMA ===");
                Console.WriteLine("1. Lihat Daftar Barang");
                Console.WriteLine("2. Tambah Barang");
                Console.WriteLine("3. Edit Barang");
                Console.WriteLine("4. Hapus Barang");
                Console.WriteLine("5. Cek Barang Perlu Restok");
                Console.WriteLine("6. Transaksi Barang Masuk/Keluar");

                if (role == "Admin")
                {
                    Console.WriteLine("7. Manajemen User");
                    Console.WriteLine("8. Laporan & Rekapitulasi");
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
                            Console.WriteLine("❌ ID tidak valid.");
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
                            Console.WriteLine("❌ ID tidak valid.");
                        }
                        break;
                    case "5":
                        inventoryManager.CheckRestockItems();
                        break;
                    case "6":
                        inventoryManager.AddItem(); // Pastikan ada metode transaksi barang
                        break;
                    case "7":
                        if (role == "Admin")
                            UserManagementMenu(userManager);
                        else
                            Console.WriteLine("❌ Anda tidak memiliki akses ke menu ini.");
                        break;
                    case "8":
                        if (role == "Admin")
                            ReportMenu(transactionManager);
                        else
                            Console.WriteLine("❌ Anda tidak memiliki akses ke menu ini.");
                        break;
                    case "0":
                        userManager.Logout();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("❌ Pilihan tidak valid.");
                        break;
                }
            }

            Console.WriteLine("Terima kasih telah menggunakan sistem ini.");
        }

        // MENU MANAJEMEN USER
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
                            Console.Write("Username baru (kosongkan jika tidak diubah): ");
                            string username = Console.ReadLine();

                            Console.Write("Password baru (kosongkan jika tidak diubah): ");
                            string password = Console.ReadLine();

                            Console.Write("Role baru (Admin/Employee) (kosongkan jika tidak diubah): ");
                            string role = Console.ReadLine();

                            userManager.EditUser(userId, userId, username, password, role);
                        }
                        else
                        {
                            Console.WriteLine("❌ ID tidak valid.");
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
                            Console.WriteLine("❌ ID tidak valid.");
                        }
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("❌ Pilihan tidak valid.");
                        break;
                }
            }
        }

        // MENU LAPORAN
        static void ReportMenu(TransactionManager transactionManager)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n=== MENU LAPORAN ===");
                Console.WriteLine("1. Laporan Harian");
                Console.WriteLine("2. Laporan Bulanan");
                Console.WriteLine("0. Kembali");

                Console.Write("\nPilih: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        Console.Write("Tanggal (yyyy-mm-dd): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                        {
                            transactionManager.GenerateDailyReport(date);
                        }
                        else
                        {
                            Console.WriteLine("❌ Format tanggal tidak valid.");
                        }
                        break;
                    case "2":
                        Console.Write("Tahun (yyyy): ");
                        if (int.TryParse(Console.ReadLine(), out int year))
                        {
                            Console.Write("Bulan (1-12): ");
                            if (int.TryParse(Console.ReadLine(), out int month) && month >= 1 && month <= 12)
                            {
                                transactionManager.GenerateMonthlyReport(year, month);
                            }
                            else
                            {
                                Console.WriteLine("❌ Bulan tidak valid.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("❌ Tahun tidak valid.");
                        }
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("❌ Pilihan tidak valid.");
                        break;
                }
            }
        }
    }
}

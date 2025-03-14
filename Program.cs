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
            var transactionManager = new TransactionManager();

            string role = ""; // Inisialisasi variabel role
            int userId = 0;   // Inisialisasi userId
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
                Console.Clear();
                Console.WriteLine($"\n=== MENU ({role.ToUpper()}) ===");

                if (role == "admin")
                {
                    Console.WriteLine("1. Lihat Daftar User");
                    Console.WriteLine("2. Tambah User");
                    Console.WriteLine("3. Edit User");
                    Console.WriteLine("4. Hapus User");
                    Console.WriteLine("5. Manajemen Transaksi");
                }
                else if (role == "Employer")
                {
                    Console.WriteLine("1. Lihat Daftar Barang");
                    Console.WriteLine("2. Tambah Barang");
                    Console.WriteLine("3. Edit Barang");
                    Console.WriteLine("4. Hapus Barang");
                    Console.WriteLine("5. Cek Barang Perlu Restok");
                    Console.WriteLine("6. Manajemen Transaksi");
                }

                Console.WriteLine("7. Edit Akun Saya");
                Console.WriteLine("0. Logout & Keluar");
                Console.Write("\nPilih menu: ");
                string pilihan = Console.ReadLine() ?? "";

                switch (pilihan)
                {
                    case "1":
                        if (role == "admin") userManager.DisplayUsers();
                        else inventoryManager.ListItems();
                        break;
                    case "2":
                        if (role == "admin") userManager.AddUser();
                        else inventoryManager.AddItem();
                        break;
                    case "3":
                        if (role == "admin")
                        {
                            Console.Write("Masukkan ID User yang ingin di-edit: ");
                            if (int.TryParse(Console.ReadLine(), out int editUserId))
                            {
                                EditUser(userManager, editUserId);
                            }
                            else
                            {
                                Console.WriteLine("ID tidak valid.");
                            }
                        }
                        else
                        {
                            Console.Write("Masukkan ID Barang yang ingin di-edit: ");
                            if (int.TryParse(Console.ReadLine(), out int editItemId))
                            {
                                inventoryManager.EditItem(editItemId);
                            }
                            else
                            {
                                Console.WriteLine("ID tidak valid.");
                            }
                        }
                        break;
                    case "4":
                        if (role == "admin")
                        {
                            Console.Write("Masukkan ID User yang ingin dihapus: ");
                            if (int.TryParse(Console.ReadLine(), out int deleteUserId))
                            {
                                userManager.DeleteUserById(deleteUserId);
                            }
                            else
                            {
                                Console.WriteLine("ID tidak valid.");
                            }
                        }
                        else
                        {
                            Console.Write("Masukkan ID Barang yang ingin dihapus: ");
                            if (int.TryParse(Console.ReadLine(), out int deleteItemId))
                            {
                                inventoryManager.DeleteItem(deleteItemId);
                            }
                            else
                            {
                                Console.WriteLine("ID tidak valid.");
                            }
                        }
                        break;
                    case "5":
                        if (role == "admin" || role == "Employer")
                        {
                            TransactionManagementMenu(transactionManager);
                        }
                        else
                        {
                            Console.WriteLine("Pilihan tidak valid.");
                        }
                        break;
                    case "6":
                        if (role == "Employer") 
                        {
                            TransactionManagementMenu(transactionManager);
                        }
                        else 
                        {
                            EditUser(userManager, userId);
                        }
                        break;
                    case "7":
                        EditUser(userManager, userId);
                        break;
                    case "0":
                        userManager.Logout();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }
                Console.WriteLine("\nTekan ENTER untuk melanjutkan...");
                Console.ReadLine();
            }

            Console.WriteLine("Terima kasih telah menggunakan sistem ini.");
        }

        static void EditUser(UserManager userManager, int userId)
        {
            Console.WriteLine("\n=== Edit Akun ===");
            Console.Write("Masukkan username baru (kosongkan jika tidak ingin mengubah): ");
            string? newUsername = Console.ReadLine();
            Console.Write("Masukkan password baru (kosongkan jika tidak ingin mengubah): ");
            string? newPassword = Console.ReadLine();

            userManager.EditUser(userId, userId, newUsername, newPassword, null);
        }

        static void TransactionManagementMenu(TransactionManager transactionManager)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\n=== MENU MANAJEMEN TRANSAKSI ===");
                Console.WriteLine("1. Tambah Transaksi");
                Console.WriteLine("2. Lihat Riwayat Transaksi");
                Console.WriteLine("3. Buat Laporan Rekapitulasi");
                Console.WriteLine("4. Ekspor Laporan ke File");
                Console.WriteLine("0. Kembali");

                Console.Write("\nPilih menu: ");
                string pilihan = Console.ReadLine() ?? "";

                switch (pilihan)
                {
                    case "1":
                        transactionManager.AddTransactions();
                        break;
                    case "2":
                        transactionManager.ViewTransactionHistory();
                        break;
                    case "3":
                        transactionManager.GenerateReport();
                        break;
                    case "4":
                        transactionManager.ExportReport();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }
                if (!back)
                {
                    Console.WriteLine("\nTekan ENTER untuk kembali ke menu transaksi...");
                    Console.ReadLine();
                }
            }
        }
    }
}
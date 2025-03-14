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
                }
                else if (role == "Employer")
                {
                    Console.WriteLine("1. Lihat Daftar Barang");
                    Console.WriteLine("2. Tambah Barang");
                    Console.WriteLine("3. Edit Barang");
                    Console.WriteLine("4. Hapus Barang");
                    Console.WriteLine("5. Cek Barang Perlu Restok");
                    Console.WriteLine("6. Kurangi Stok Barang");
                    Console.WriteLine("7. auto menambahkan Stok Barang");
                }

                Console.WriteLine("8. Edit Akun Saya");
                Console.WriteLine("0. Logout & Keluar");
                Console.Write("\nPilih menu: ");
                string pilihan = Console.ReadLine() ?? "";

                switch (pilihan)
                {
                    // === ADMIN MENU ===
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
                        if (role == "Employer") inventoryManager.CheckRestockItems();
                        else Console.WriteLine("Pilihan tidak valid.");
                        break;
                    case "6": // Fungsi DecreaseItem
                        if (role == "Employer")
                        {
                            Console.Write("Masukkan ID Barang yang stoknya akan dikurangi: ");
                            if (int.TryParse(Console.ReadLine(), out int decreaseItemId))
                            {
                                inventoryManager.DecreaseItem(decreaseItemId);
                            }
                            else
                            {
                                Console.WriteLine("ID tidak valid.");
                            }
                        }
                        else Console.WriteLine("Pilihan tidak valid.");
                        break;

                    //ini tambahan dari opal
                    case "7":
                        if (role == "Employer")
                        {
                            Console.Write("Masukkan ID Barang untuk di-auto-restock: ");
                            if (int.TryParse(Console.ReadLine(), out int restockItemId))
                            {
                                var item = inventoryManager.GetItemById(restockItemId);
                                if (item != null)
                                {
                                    inventoryManager.AutoRestock(item); // Panggil dengan parameter
                                }
                                else
                                {
                                    Console.WriteLine("❌ Barang tidak ditemukan.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("❌ ID tidak valid.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Pilihan tidak valid.");
                        }
                        break;


                    case "8":
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
    }
}

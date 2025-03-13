using System;
using InventorySystem.Services;

namespace InventorySystem
{
    class Program
    {
        static void Main()
        {
            UserManager userManager = new UserManager();

            Console.WriteLine("=== Aplikasi Manajemen User ===\n");
            userManager.DisplayUsers();

            Console.Write("\nMasukkan ID User yang ingin dihapus: ");
            string? inputId = Console.ReadLine();

            if (int.TryParse(inputId, out int userId))
            {
                Console.Write("Masukkan nama admin yang melakukan penghapusan: ");
                string? adminName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(adminName))
                {
                    userManager.DeleteUserById(userId, adminName);
                }
                else
                {
                    Console.WriteLine("Nama admin tidak boleh kosong.");
                }
            }
            else
            {
                Console.WriteLine("ID tidak valid. Harus berupa angka.");
            }

            Console.WriteLine("Daftar User setelah update:");
            userManager.DisplayUsers();
        }
    }
}

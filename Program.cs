using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        UserManager userManager = new UserManager();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Menu ===");
            Console.WriteLine("1. Tambah User");
            Console.WriteLine("2. Edit User");
            Console.WriteLine("3. Login");
            Console.WriteLine("4. Keluar");
            Console.Write("Pilih opsi (1-4): ");
            string option = Console.ReadLine() ?? "";

            if (option == "1")
            {
                userManager.UserAdd();
            }
            else if (option == "2")
            {
                Console.Write("Masukkan ID Anda: ");
                if (!int.TryParse(Console.ReadLine(), out int editorId))
                {
                    Console.WriteLine("ID tidak valid!");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Masukkan ID User yang ingin diedit: ");
                if (!int.TryParse(Console.ReadLine(), out int userId))
                {
                    Console.WriteLine("ID tidak valid!");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Masukkan Username baru (kosongkan jika tidak ingin mengubah): ");
                string? newUsername = Console.ReadLine();

                Console.Write("Masukkan Password baru (kosongkan jika tidak ingin mengubah): ");
                string? newPassword = Console.ReadLine();

                Console.Write("Masukkan Role baru (Admin/Employee, kosongkan jika tidak ingin mengubah): ");
                string? newRole = Console.ReadLine();

                bool success = userManager.EditUser(editorId, userId, newUsername, newPassword, newRole);
                if (success)
                {
                    Console.WriteLine("User berhasil diperbarui!");
                }
                else
                {
                    Console.WriteLine("Gagal memperbarui user.");
                }
                Console.ReadKey();
            }
            else if (option == "3")
            {
                Console.Write("Masukkan username: ");
                string username = Console.ReadLine() ?? "";

                Console.Write("Masukkan password: ");
                string password = Console.ReadLine() ?? "";

                bool isSuccess = userManager.Login(username, password);

                if (!isSuccess)
                {
                    Console.WriteLine("Login gagal. Tekan tombol apapun untuk kembali ke menu...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Login sukses. Tekan tombol apapun untuk keluar...");
                    Console.ReadKey();
                    break;
                }
            }
            else if (option == "4")
            {
                Console.WriteLine("Keluar dari program...");
                break;
            }
            else
            {
                Console.WriteLine("Pilihan tidak valid. Tekan tombol apapun untuk kembali...");
                Console.ReadKey();
            }
        }
    }
}
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
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Keluar");
            Console.Write("Pilih opsi (1-3): ");
            string option = Console.ReadLine() ?? "";

            if (option == "1")
            {
                userManager.UserAdd();
            }
            else if (option == "2")
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
            else if (option == "3")
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

using System;

class Program
{
    static void Main()
    {
        UserManager userManager = new UserManager();

        Console.Write("Masukkan username: ");
        string username = Console.ReadLine();

        Console.Write("Masukkan password: ");
        string password = Console.ReadLine();

        
        if (userManager.Login(username, password))
        {
            Console.WriteLine("Login sukses!");

            Console.WriteLine("Tekan ENTER untuk logout...");
            Console.ReadLine();
            userManager.Logout();
        }
        else
        {
            Console.WriteLine("Login gagal.");
        }
    }
}

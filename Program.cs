using System;
using InventorySystem.Services;

class Program
{
    static void Main()
    {
        UserManager userManager = new UserManager();

        Console.Write("Masukkan ID user yang ingin diedit: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Console.Write("Masukkan nama baru (biarkan kosong jika tidak ingin mengubah): ");
            string? newName = Console.ReadLine();

            Console.Write("Masukkan role baru (Admin/Employee, kosong jika tidak ingin mengubah): ");
            string? newRole = Console.ReadLine();

            userManager.EditUser(id, newName, newRole);
        }
        else
        {
            Console.WriteLine("ID tidak valid!");
        }
    }
}
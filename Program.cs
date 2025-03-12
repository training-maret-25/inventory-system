using InventorySystem.Services;
using InventorySystem.Models;

namespace InventorySystem
{
    static class Program
    {
        static void Main()
        {
            HashConverter.ConvertPasswords();

            UserManager userManager = new UserManager();

            Console.Write("Masukkan username Admin: ");
            string adminUsername = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Masukkan password Admin: ");
            string adminPassword = Console.ReadLine()?.Trim() ?? "";

            User? adminUser = userManager.Authenticate(adminUsername, adminPassword);

            if (adminUser == null || adminUser.Role != "Admin")
            {
                Console.WriteLine("Login gagal! Hanya Admin yang bisa mengedit user.");
                return;
            }

            Console.Write("Masukkan ID user yang ingin diedit: ");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.Write("Masukkan username baru (biarkan kosong jika tidak ingin mengubah): ");
                string newUsername = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Masukkan password baru (biarkan kosong jika tidak ingin mengubah): ");
                string newPassword = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Masukkan role baru (Admin/Employee, kosong jika tidak ingin mengubah): ");
                string newRole = Console.ReadLine()?.Trim() ?? "";

                // Validasi role hanya jika diisi
                if (!string.IsNullOrEmpty(newRole) && newRole != "Admin" && newRole != "Employee")
                {
                    Console.WriteLine("Role tidak valid! Gunakan 'Admin' atau 'Employee'.");
                    return;
                }

                userManager.EditUser(adminUser.Id, userId, newUsername, newPassword, newRole);
            }
            else
            {
                Console.WriteLine("ID tidak valid!");
            }
        }
    }
}
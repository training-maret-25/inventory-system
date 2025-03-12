using Newtonsoft.Json;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public class UserManager
    {
        private readonly string filePath = "data/user.json";
        private readonly List<User> users;

        public UserManager()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }
            else
            {
                users = new List<User>();
            }
        }

        // Simpan data user
        public void SaveUsers()
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public User? Authenticate(string username, string password) // Pastikan ini sesuai
        {
            User? user = users.Find(u => u.Username == username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }
            return null;
        }

        // Edit data user
        public bool EditUser(int adminId, int userId, string? newUsername, string? newPassword, string? newRole)
        {
            User? adminUser = users.Find(u => u.Id == adminId);
            if (adminUser == null || adminUser.Role != "Admin")
            {
                Console.WriteLine("Izin ditolak! Hanya Admin yang bisa mengedit user.");
                return false;
            }

            User? user = users.Find(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine($"User dengan ID {userId} tidak ditemukan.");
                return false;
            }

            if (!string.IsNullOrEmpty(newUsername))
            {
                user.Username = newUsername;
            }
            if (!string.IsNullOrEmpty(newPassword))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            }
            if (!string.IsNullOrEmpty(newRole) && (newRole == "Admin" || newRole == "Employee"))
            {
                user.Role = newRole;
            }

            SaveUsers();
            Console.WriteLine($"User dengan ID {userId} berhasil diperbarui!");
            return true;
        }
    }
}
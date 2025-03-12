using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public class UserManager
    {
        private string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "users.json");
        private List<User> users = new();

        public UserManager()
        {
            Console.WriteLine($"Path users.json: {filePath}");
            LoadUsers();
        }

        private void LoadUsers()
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                users = JsonSerializer.Deserialize<List<User>>(jsonData) ?? new List<User>();
            }
            else
            {
                Console.WriteLine($"File users.json tidak ditemukan di path: {filePath}");
            }
        }

        private void SaveUsers()
        {
            string jsonData = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }

        public void DisplayUsers()
        {
            Console.WriteLine("\n=== Daftar User ===");
            if (users.Count == 0)
            {
                Console.WriteLine("Tidak ada data user.");
                return;
            }

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Username: {user.Username}, Role: {user.Role}");
            }
        }

        public void DeleteUserById(int userId, string adminName)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                Console.WriteLine("User tidak ditemukan!");
                return;
            }

            users.Remove(user);
            SaveUsers();

            Console.WriteLine($"User '{user.Username}' berhasil dihapus oleh Admin '{adminName}'.");
        }
    }
}

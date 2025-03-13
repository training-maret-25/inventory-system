using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public class UserManager
    {
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "users.json");
        private List<User> users = new();
        private User? _currentUser = null;

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
                CheckAndHashPasswords(); // Pastikan semua password sudah hash
            }
            else
            {
                Console.WriteLine("⚠️ File users.json tidak ditemukan! Membuat daftar user kosong.");
            }
        }

        private void SaveUsers()
        {
            string jsonData = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }

        // Pastikan semua password sudah hash SHA-256
        private void CheckAndHashPasswords()
        {
            bool updated = false;
            foreach (var user in users)
            {
                if (!IsValidHash(user.Password))
                {
                    Console.WriteLine($"🔹 Password user '{user.Username}' belum di-hash. Mengupdate...");
                    user.Password = HashPassword(user.Password);
                    updated = true;
                }
            }

            if (updated)
            {
                SaveUsers();
                Console.WriteLine("✅ Semua password telah di-hash dan file diperbarui.");
            }
        }

        private static bool IsValidHash(string password)
        {
            return password.Length == 64 && password.All(c => "0123456789abcdefABCDEF".Contains(c));
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        // Login user
        public bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("❌ Username dan password tidak boleh kosong!");
                return false;
            }

            var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                Console.WriteLine("❌ Username tidak ditemukan!");
                return false;
            }

            string hashedInputPassword = HashPassword(password);

            if (!user.Password.Equals(hashedInputPassword, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("❌ Password salah!");
                return false;
            }

            _currentUser = user;
            Console.WriteLine($"✅ Login berhasil! Selamat datang, {user.Username} ({user.Role}).");
            return true;
        }

        // Logout user
        public void Logout()
        {
            if (_currentUser != null)
            {
                Console.WriteLine($"✅ {_currentUser.Username} telah logout.");
                _currentUser = null;
            }
            else
            {
                Console.WriteLine("⚠️ Tidak ada user yang login.");
            }
        }

        // Menampilkan semua user
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

        // Menambahkan user baru
        public void AddUser()
        {
            Console.Write("Masukkan username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Masukkan password: ");
            string password = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("❌ Username dan password tidak boleh kosong!");
                return;
            }

            if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("❌ Username sudah ada!");
                return;
            }

            var newUser = new User
            {
                Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1,
                Username = username,
                Password = HashPassword(password),
                Role = "Employer" // Default role
            };

            users.Add(newUser);
            SaveUsers();
            Console.WriteLine("✅ User berhasil ditambahkan!");
        }

        // Edit user berdasarkan ID (Admin bisa edit semua user, user hanya bisa edit diri sendiri)
        public bool EditUser(int editorId, int userId, string? newUsername, string? newPassword, string? newRole)
        {
            User? editor = users.Find(u => u.Id == editorId);
            if (editor == null)
            {
                Console.WriteLine("User tidak ditemukan.");
                return false;
            }

            User? user = users.Find(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine($"User dengan ID {userId} tidak ditemukan.");
                return false;
            }

            // Admin bisa edit semua user, Employee hanya bisa edit dirinya sendiri
            if (editor.Role != "Admin" && editor.Id != userId)
            {
                Console.WriteLine("Izin ditolak! Anda hanya bisa mengedit akun Anda sendiri.");
                return false;
            }

            if (!string.IsNullOrEmpty(newUsername))
            {
                user.Username = newUsername;
            }
            if (!string.IsNullOrEmpty(newPassword))
            {
                user.Password = HashPassword(newPassword);
            }
            if (!string.IsNullOrEmpty(newRole) && (newRole == "Admin" || newRole == "Employee"))
            {
                // Role hanya bisa diubah oleh Admin
                if (editor.Role == "Admin")
                {
                    user.Role = newRole;
                }
                else
                {
                    Console.WriteLine("Izin ditolak! Hanya Admin yang bisa mengubah role.");
                    return false;
                }
            }

            UpdateUsers();
            Console.WriteLine($"User dengan ID {userId} berhasil diperbarui!");
            return true;
        }

        private void UpdateUsers()
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(UserFile, json);
        }

        // Menghapus user berdasarkan ID (hanya bisa dilakukan oleh Admin)
        public void DeleteUserById(int userId)
        {
            if (_currentUser == null || !_currentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("❌ Hanya Admin yang bisa menghapus user!");
                return;
            }

            var user = users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                Console.WriteLine("❌ User tidak ditemukan!");
                return;
            }

            users.Remove(user);
            SaveUsers();

            Console.WriteLine($"✅ User '{user.Username}' berhasil dihapus oleh Admin '{_currentUser.Username}'.");
        }
    }
}

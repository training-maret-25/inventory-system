using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using InventorySystem.Models;
using logger;
using InventorySystem.Services;

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
                CheckAndHashPasswords(); 
            }
            else
            {
                Console.WriteLine("File users.json tidak ditemukan! Membuat daftar user kosong.");
                Logger.LogError("file users.json tidak ditemukan. sistem memulai dengan daftar user kosong");
            }
        }

        private void SaveUsers()
        {
            string jsonData = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }
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
                Console.WriteLine("Semua password telah di-hash dan file diperbarui.");
                Logger.LogInfo("SYSTEM", "Hashing ulang password yang belum ter-hash");
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
        public bool Login(string username, string password, out string role, out int userId)
        {
            role = "";
            userId = -1;

            var user = users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                Console.WriteLine("Username tidak ditemukan!");
                Logger.LogError("Gagal Login: Username tidak ditemukan");
                return false;
            }

            string hashedInputPassword = HashPassword(password);

            if (!user.Password.Equals(hashedInputPassword, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Password salah!");
                Logger.LogError( "Gagal Login: Password salah");
                return false;
            }
            _currentUser = user;
            role = user.Role.Trim();  
            userId = user.Id;

            Console.WriteLine($"Login berhasil! Selamat datang, {user.Username} ({user.Role}).");
            Logger.LogInfo(username, "login ke sistem");
            return true;
        }

        // Logout user
        public void Logout()
        {
            if (_currentUser != null)
            {
                Console.WriteLine($"{_currentUser.Username} telah logout.");
                Logger.LogInfo(_currentUser.Username, "telah logout");
                _currentUser = null;
            }
            else
            {
                Console.WriteLine("Tidak ada user yang login.");
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

            Logger.LogInfo(_currentUser.Username, "menampilkan daftar nama user");
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
                Console.WriteLine("Username dan password tidak boleh kosong!");
                Logger.LogError("Gagal memasukan user: username dan password kosong");
                return;
            }

            if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Username sudah ada!");
                Logger.LogError($"Gagal memasukan user: Username {username} sudah ada");
                return;
            }

            var newUser = new User
            {
                Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1,
                Username = username,
                Password = HashPassword(password),
                Role = "Employer" 
            };

            users.Add(newUser);
            SaveUsers();
            Console.WriteLine("User berhasil ditambahkan!");
            Logger.LogInfo("ADMIN", $"Menambahkan user baru: {username}");
        }

        public bool EditUser(int editorId, int userId, string? newUsername, string? newPassword, string? newRole)
        {
            User? editor = users.Find(u => u.Id == editorId);
            if (editor == null)
            {
                Console.WriteLine("User tidak ditemukan.");
                Logger.LogError($"Gagal mengedit user: User ID{userId} tidak ditemukan");
                return false;
            }

            User? user = users.Find(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine($"User dengan ID {userId} tidak ditemukan.");
                Logger.LogError($"Gagal mengedit user: User ID {userId} tidak ditemukan.");
                return false;
            }

            if (editor.Role == "Admin" && editor.Id != userId)
            {
                Console.WriteLine("Izin ditolak! Anda hanya bisa mengedit akun Anda sendiri.");
                Logger.LogWarning(editor.Username, "Gagal mengedit user: Izin ditolak.");
                return false;
            }

            bool updated = false;
            List<string> changes = new List<string>();

            if (!string.IsNullOrEmpty(newUsername))
            {
                changes.Add($"Username dari '{user.Username}' ke '{newUsername}'");
                user.Username = newUsername;
                updated = true;
            }
            if (!string.IsNullOrEmpty(newPassword))
            {
                user.Password = HashPassword(newPassword);
                changes.Add("Password diperbarui");
                updated = true;
            }
            if (!string.IsNullOrEmpty(newRole) && (newRole == "Admin" || newRole == "Employee"))
            {
                if (editor.Role == "Admin")
                {
                    changes.Add($"Role dari '{user.Role}' ke '{newRole}'");
                    user.Role = newRole;
                    updated = true;
                }
                else
                {
                    Console.WriteLine("Izin ditolak! Hanya Admin yang bisa mengubah role.");
                    Logger.LogWarning(editor.Username, "Mencoba mencoba mengubah role.");
                    return false;
                }
            }

            if (updated)
            {
                UpdateUsers();
                Console.WriteLine($"User dengan ID {userId} berhasil diperbarui!");
                string action = $"Mengedit user ID {userId} ({string.Join(", ", changes)})";
                Logger.LogInfo(editor.Username, action);
                return true;
            }
            return false;
        }

        private void UpdateUsers()
        {
            string jsonData = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }

        public void DeleteUserById(int userId)
        {
            if (_currentUser == null || !_currentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Hanya Admin yang bisa menghapus user!");
                Logger.LogError($"Percobaan penghapusan user ID {userId} oleh non-admin.");

                return;
            }

            var user = users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                Console.WriteLine("User tidak ditemukan!");
                Logger.LogWarning(_currentUser.Username, $"Percobaan penghapusan user ID {userId}, tetapi user tidak ditemukan.");
                return;
            }

            users.Remove(user);
            SaveUsers();

            Console.WriteLine($"User '{user.Username}' berhasil dihapus oleh Admin '{_currentUser.Username}'.");
            Logger.LogInfo(_currentUser.Username, $"Menghapus user ID {userId} ({user.Username})");
        }
    }
}

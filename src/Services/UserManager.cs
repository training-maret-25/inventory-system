using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using InventorySystem.Models;
using logger;

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
                Logger.LogError("SYSTEM", "file users.json tidak ditemukan. sistem memulai dengan daftar user kosong");
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
                Logger.LogUserModification("SYSTEM", "Hashing ulang password yang belum ter-hash");
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
                Console.WriteLine("❌ Username tidak ditemukan!");
                Logger.LogError(username, "Gagal Login: Username tidak ditemukan");
                return false;
            }

            string hashedInputPassword = HashPassword(password);

            if (!user.Password.Equals(hashedInputPassword, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("❌ Password salah!");
                Logger.LogError(username, "Gagal Login: Password salah");
                return false;
            }

            // Jika berhasil login, simpan user & role
            _currentUser = user;
            role = user.Role.Trim();  // Pastikan tidak ada spasi yang mengganggu
            userId = user.Id;

            Console.WriteLine($"✅ Login berhasil! Selamat datang, {user.Username} ({role}).");  // Debugging
            Logger.LogLogin(username);
            return true;
        }


        // Logout user
        public void Logout()
        {
            if (_currentUser != null)
            {
                Console.WriteLine($"✅ {_currentUser.Username} telah logout.");
                Logger.LogLogout(_currentUser.Username);
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
                Logger.LogError("SYSTEM", "Gagal memasukan user: username dan password kosong");
                return;
            }

            if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("❌ Username sudah ada!");
                Logger.LogError("SYSTEM", $"Gagal memasukan user: Username '{username}' sudah ada");
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
            Logger.LogUserModification("ADMIN", $"Menambahkan user baru: {username}");
        }

        // Edit user berdasarkan ID (Admin bisa edit semua user, user hanya bisa edit diri sendiri)
        public bool EditUser(int editorId, int userId, string? newUsername, string? newPassword, string? newRole)
        {
            User? editor = users.Find(u => u.Id == editorId);
            if (editor == null)
            {
                Console.WriteLine("User tidak ditemukan.");
                Logger.LogError(editor.Username, $"Gagal mengedit user: User ID{userId} tidak ditemukan");
                return false;
            }

            User? user = users.Find(u => u.Id == userId);
            if (user == null)
            {
                Console.WriteLine($"User dengan ID {userId} tidak ditemukan.");
                Logger.LogError(editor.Username, $"Gagal mengedit user: User ID {userId} tidak ditemukan.");
                return false;
            }

            // Admin bisa edit semua user, Employee hanya bisa edit dirinya sendiri
            if (editor.Role == "Admin" && editor.Id != userId)
            {
                Console.WriteLine("Izin ditolak! Anda hanya bisa mengedit akun Anda sendiri.");
                Logger.LogError(editor.Username, "Gagal mengedit user: Izin ditolak.");
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
                // Role hanya bisa diubah oleh Admin
                if (editor.Role == "Admin")
                {
                    changes.Add($"Role dari '{user.Role}' ke '{newRole}'");
                    user.Role = newRole;
                    updated = true;
                }
                else
                {
                    Console.WriteLine("Izin ditolak! Hanya Admin yang bisa mengubah role.");
                    Logger.LogError(editor.Username, "Gagal mengubah role user: Izin ditolak.");
                    return false;
                }
            }

            if (updated) {
                UpdateUsers();
                Console.WriteLine($"User dengan ID {userId} berhasil diperbarui!");
                string action = $"Mengedit user ID {userId} ({string.Join(", ", changes)})";
                Logger.LogUserModification(editor.Username, action);
                return true;
            } 
            return false;
        }

        private void UpdateUsers()
        {
            string jsonData = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }


        // Menghapus user berdasarkan ID (hanya bisa dilakukan oleh Admin)
        public void DeleteUserById(int userId)
        {
            if (_currentUser == null || !_currentUser.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("❌ Hanya Admin yang bisa menghapus user!");
                Logger.LogError("SYSTEM", $"Percobaan penghapusan user ID {userId} oleh non-admin.");

                return;
            }

            var user = users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                Console.WriteLine("❌ User tidak ditemukan!");
                Logger.LogError("SYSTEM", $"Percobaan penghapusan user ID {userId}, tetapi user tidak ditemukan.");
                return;
            }

            users.Remove(user);
            SaveUsers();

            Console.WriteLine($"✅ User '{user.Username}' berhasil dihapus oleh Admin '{_currentUser.Username}'.");
            Logger.LogUserModification(_currentUser.Username, $"Menghapus user ID {userId} ({user.Username})");
        }
    }
}
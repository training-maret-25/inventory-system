using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Harus SHA-256 hash
    public string Role { get; set; } = string.Empty;
}

public class UserManager
{
    private const string UserFile = "data/users.json"; // sesuaikan pathnya
    private List<User> users = new List<User>();
    private User? _currentUser = null;

    public UserManager()
    {
        LoadUsers();
    }

    private void LoadUsers()
    {
        if (File.Exists(UserFile))
        {
            string json = File.ReadAllText(UserFile);
            users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            CheckAndHashPasswords();  // Pastikan semua password sudah di-hash
        }
        else
        {
            Console.WriteLine("File users.json tidak ditemukan! Membuat daftar user kosong.");
            users = new List<User>();
        }
    }

    private void CheckAndHashPasswords()
    {
        bool updated = false;

        foreach (var user in users)
        {
            if (!IsValidHash(user.Password))
            {
                Console.WriteLine($"Password untuk {user.Username} belum di-hash. Mengupdate...");
                user.Password = HashPassword(user.Password);
                updated = true;
            }
        }

        if (updated)
        {
            File.WriteAllText(UserFile, JsonConvert.SerializeObject(users, Formatting.Indented));
            Console.WriteLine("Semua password telah di-hash dan JSON diperbarui!");
        }
    }

    private static bool IsValidHash(string password)
    {
        return password.Length == 64 && IsHex(password);
    }

    private static bool IsHex(string input)
    {
        foreach (char c in input)
        {
            if (!Uri.IsHexDigit(c))
                return false;
        }
        return true;
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower(); // Pakai lowercase agar konsisten
        }
    }

    public bool Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Username dan password tidak boleh kosong!");
            return false;
        }

        string hashedPassword = HashPassword(password);
        var user = users.Find(u => u.Username == username);

        if (user == null)
        {
            Console.WriteLine("Username tidak ditemukan!");
            return false;
        }

        Console.WriteLine($"(Debug) Hash dari input: {hashedPassword}");
        Console.WriteLine($"(Debug) Hash dari database: {user.Password}");

        if (!user.Password.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Password salah!");
            return false;
        }

        _currentUser = user;
        Console.WriteLine($"Login berhasil! Selamat datang, {user.Username} ({user.Role}).");
        return true;
    }

    public void Logout()
    {
        if (_currentUser != null)
        {
            Console.WriteLine($"{_currentUser.Username} telah logout.");
            _currentUser = null;
        }
        else
        {
            Console.WriteLine("Tidak ada user yang sedang login.");
        }
    }

    public void UserAdd()
    {
        Console.Write("Masukkan username: ");
        string username = Console.ReadLine() ?? "";

        Console.Write("Masukkan password: ");
        string password = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Username dan password tidak boleh kosong!");
            return;
        }

        // Cek apakah username sudah ada
        if (users.Exists(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Username sudah ada!");
            return;
        }

        // Buat pengguna baru
        User newUser = new User
        {
            Id = users.Count > 0 ? users[^1].Id + 1 : 1, // Id otomatis (increment)
            Username = username,
            Password = HashPassword(password), // Hash password
            Role = "Employer" // Atur role sebagai Employer
        };

        users.Add(newUser); // Tambahkan ke daftar pengguna
        SaveUsers(); // Simpan ke file JSON

        Console.WriteLine("User berhasil ditambahkan!");
    }

    private void SaveUsers()
    {
        string json = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(UserFile, json);
    }

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
}
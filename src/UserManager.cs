using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; } 
    public string? Role { get; set; } 
}

public class UserManager
{
    private const string UserFile = @"C:\Users\ASUS\source\repos\inventory-system\data\users.json"; 
    private List<User> users = new List<User>(); 
    private User? _currentUser = null; 

    private void LoadUsers()
    {
        try
        {
            if (File.Exists(UserFile))
            {
                string json = File.ReadAllText(UserFile);
                users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }
            else
            {
                Console.WriteLine("File users.json tidak ditemukan! Membuat daftar pengguna kosong.");
                users = new List<User>();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Kesalahan saat membaca file: {ex.Message}");
            users = new List<User>();
        }
    }

    public bool Login(string username, string password)
    {
        try
        {
            LoadUsers();
            var user = users.Find(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                _currentUser = user;
                Console.WriteLine($"Login berhasil! Selamat datang, {user.Username} ({user.Role}).");
                return true;
            }
            else
            {
                Console.WriteLine("Username atau password salah!");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Kesalahan saat login: {ex.Message}");
            return false;
        }
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
}

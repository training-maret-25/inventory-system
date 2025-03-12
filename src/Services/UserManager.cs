using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using InventorySystem.Models; 

namespace InventorySystem.Services
{
    public class UserManager
    {
        private string filePath = "data/user.json";
        private List<User> users;

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

        public void SaveUsers()
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void EditUser(int id, string? newName, string? newRole)
        {
            // Cari user berdasarkan ID
            User? user = users.Find(u => u.Id == id);

            if (user == null)
            {
                Console.WriteLine("User tidak ditemukan!");
                return;
            }

            // Update nama dan role jika tidak null atau kosong
            if (!string.IsNullOrWhiteSpace(newName))
            {
                user.Username = newName;
            }
            if (!string.IsNullOrWhiteSpace(newRole))
            {
                user.Role = newRole;
            }

            SaveUsers();
            Console.WriteLine($"User dengan ID {id} berhasil diperbarui!");
        }
    }
}
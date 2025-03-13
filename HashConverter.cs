namespace InventorySystem.Services
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using Newtonsoft.Json;
  using BCrypt.Net;
  using InventorySystem.Models;

  public static class HashConverter
  {
    public static void ConvertPasswords()
    {
      string filePath = "data/user.json";

      if (!File.Exists(filePath))
      {
        Console.WriteLine("File user.json tidak ditemukan!");
        return;
      }

      string json = File.ReadAllText(filePath);
      List<User> users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();

      foreach (var user in users)
      {
        if (!user.Password.StartsWith("$2b$")) // Cek apakah password sudah di-hash
        {
          user.Password = BCrypt.HashPassword(user.Password);
        }
      }

      File.WriteAllText(filePath, JsonConvert.SerializeObject(users, Formatting.Indented));
      Console.WriteLine("Semua password berhasil di-hash!");
    }
  }
}
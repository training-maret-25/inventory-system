using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public class InventoryManager
    {
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "inventory.json");
        private List<InventoryItem> inventory = new();
        private User? _currentUser;

        public InventoryManager(User? currentUser)
        {
            Console.WriteLine($"Path inventory.json: {filePath}");
            _currentUser = currentUser;
            LoadInventory();
        }

        private void LoadInventory()
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                inventory = JsonSerializer.Deserialize<List<InventoryItem>>(jsonData) ?? new List<InventoryItem>();
            }
            else
            {
                Console.WriteLine("⚠️ File inventory.json tidak ditemukan! Membuat daftar barang kosong.");
            }
        }

        private void SaveInventory()
        {
            string jsonData = JsonSerializer.Serialize(inventory, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }

        // Menampilkan semua barang
        public void DisplayItems()
        {
            Console.WriteLine("\n=== Daftar Barang ===");
            if (inventory.Count == 0)
            {
                Console.WriteLine("Tidak ada barang dalam inventory.");
                return;
            }

            foreach (var item in inventory)
            {
                Console.WriteLine($"ID: {item.Id}, Nama: {item.Name}, Stok: {item.Stock}, Harga: {item.Price}, Min Stok: {item.MinimumStock}");
            }
        }

        // Menambahkan barang baru (hanya Employee)
        public void AddItem()
        {
            if (!IsEmployee()) return;

            Console.Write("Masukkan nama barang: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Masukkan jumlah stok: ");
            if (!int.TryParse(Console.ReadLine(), out int stock)) stock = 0;

            Console.Write("Masukkan harga barang: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price)) price = 0;

            Console.Write("Masukkan stok minimum: ");
            if (!int.TryParse(Console.ReadLine(), out int minStock)) minStock = 0;

            if (string.IsNullOrWhiteSpace(name) || stock < 0 || price < 0 || minStock < 0)
            {
                Console.WriteLine("❌ Data tidak valid! Pastikan semua field terisi dan bernilai positif.");
                return;
            }

            var newItem = new InventoryItem
            {
                Id = inventory.Count > 0 ? inventory.Max(i => i.Id) + 1 : 1,
                Name = name,
                Stock = stock,
                Price = price,
                MinimumStock = minStock
            };

            inventory.Add(newItem);
            SaveInventory();
            Console.WriteLine("✅ Barang berhasil ditambahkan!");
        }

        // Mengedit barang (hanya Employee)
        public void EditItem()
        {
            if (!IsEmployee()) return;

            Console.Write("Masukkan ID barang yang ingin diedit: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ ID tidak valid!");
                return;
            }

            var item = inventory.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("❌ Barang tidak ditemukan!");
                return;
            }

            Console.Write($"Nama barang ({item.Name}): ");
            string name = Console.ReadLine() ?? item.Name;

            Console.Write($"Jumlah stok ({item.Stock}): ");
            string stockInput = Console.ReadLine()!;
            int stock = string.IsNullOrWhiteSpace(stockInput) ? item.Stock : int.Parse(stockInput);

            Console.Write($"Harga ({item.Price}): ");
            string priceInput = Console.ReadLine()!;
            decimal price = string.IsNullOrWhiteSpace(priceInput) ? item.Price : decimal.Parse(priceInput);

            Console.Write($"Stok minimum ({item.MinimumStock}): ");
            string minStockInput = Console.ReadLine()!;
            int minStock = string.IsNullOrWhiteSpace(minStockInput) ? item.MinimumStock : int.Parse(minStockInput);

            item.Name = name;
            item.Stock = stock;
            item.Price = price;
            item.MinimumStock = minStock;

            SaveInventory();
            Console.WriteLine("✅ Barang berhasil diperbarui!");
        }

        // Menghapus barang (hanya Employee)
        public void DeleteItem()
        {
            if (!IsEmployee()) return;

            Console.Write("Masukkan ID barang yang ingin dihapus: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ ID tidak valid!");
                return;
            }

            var item = inventory.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("❌ Barang tidak ditemukan!");
                return;
            }

            inventory.Remove(item);
            SaveInventory();
            Console.WriteLine($"✅ Barang '{item.Name}' berhasil dihapus.");
        }

        // Pengecekan Role Employee
        private bool IsEmployee()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("❌ Anda belum login!");
                return false;
            }

            if (!_currentUser.Role.Equals("Employee", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("❌ Hanya Employee yang bisa mengelola barang!");
                return false;
            }

            return true;
        }

        // Menampilkan barang yang stoknya di bawah minimum
        public void DisplayLowStockItems()
        {
            Console.WriteLine("\n=== Barang dengan Stok di Bawah Minimum ===");
            var lowStockItems = inventory.Where(i => i.Stock < i.MinimumStock).ToList();

            if (lowStockItems.Count == 0)
            {
                Console.WriteLine("Semua stok barang mencukupi.");
                return;
            }

            foreach (var item in lowStockItems)
            {
                Console.WriteLine($"ID: {item.Id}, Nama: {item.Name}, Stok: {item.Stock}, Min Stok: {item.MinimumStock}");
            }
        }
    }
}

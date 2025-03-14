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
        private readonly string filePathUser = Path.Combine(Directory.GetCurrentDirectory(), "data", "users.json");

        public InventoryManager()
        {
            Console.WriteLine($"Path inventory.json: {filePath}");
            LoadInventory();
        }

        private void LoadInventory()
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                inventory = JsonSerializer.Deserialize<List<InventoryItem>>(jsonData) ?? new List<InventoryItem>();
                Console.WriteLine("Inventory berhasil dimuat.");
            }
            else
            {
                Console.WriteLine("File inventory.json tidak ditemukan! Membuat daftar kosong.");
            }
        }

        private void SaveInventory()
        {
            string jsonData = JsonSerializer.Serialize(inventory, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }

        // Tambah Barang
        public void AddItem()
        {
            Console.Write("Nama Barang: ");
            string nama = Console.ReadLine() ?? "";

            Console.Write("Kategori: ");
            string kategori = Console.ReadLine() ?? "";

            Console.Write("Stok Awal: ");
            if (!int.TryParse(Console.ReadLine(), out int stok) || stok < 0)
            {
                Console.WriteLine("Stok harus berupa angka positif.");
                return;
            }

            Console.Write("Batas Minimum Stok: ");
            if (!int.TryParse(Console.ReadLine(), out int batasMinimum) || batasMinimum < 0)
            {
                Console.WriteLine("Batas minimum harus berupa angka positif.");
                return;
            }

            Console.Write("Jumlah Restok: ");
            if (!int.TryParse(Console.ReadLine(), out int jumlahRestok) || jumlahRestok < 0)
            {
                Console.WriteLine("Jumlah restok harus berupa angka positif.");
                return;
            }

            var newItem = new InventoryItem
            {
                Id = inventory.Count > 0 ? inventory.Max(i => i.Id) + 1 : 1,
                Nama = nama,
                Kategori = kategori,
                Stok = stok,
                BatasMinimum = batasMinimum,
                JumlahRestok = jumlahRestok
            };

            inventory.Add(newItem);
            SaveInventory();
            Console.WriteLine("Barang berhasil ditambahkan!");
            
        }

        // Edit Barang
        public void EditItem(int id)
        {
            var item = inventory.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("Barang tidak ditemukan.");
                return;
            }

            Console.Write($"Nama Baru ({item.Nama}): ");
            string nama = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nama)) item.Nama = nama;

            Console.Write($"Kategori Baru ({item.Kategori}): ");
            string kategori = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(kategori)) item.Kategori = kategori;

            Console.Write($"Stok Baru ({item.Stok}): ");
            if (int.TryParse(Console.ReadLine(), out int stok)) item.Stok = stok;

            Console.Write($"Batas Minimum Baru ({item.BatasMinimum}): ");
            if (int.TryParse(Console.ReadLine(), out int batasMinimum)) item.BatasMinimum = batasMinimum;

            Console.Write($"Jumlah Restok Baru ({item.JumlahRestok}): ");
            if (int.TryParse(Console.ReadLine(), out int jumlahRestok)) item.JumlahRestok = jumlahRestok;

            SaveInventory();
            Console.WriteLine("Barang berhasil diperbarui!");
        }

        // Hapus Barang
        public void DeleteItem(int id)
        {
            var item = inventory.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("Barang tidak ditemukan.");
                return;
            }
            if (item == null)
            {
                throw new ArgumentException("Jumlah yang dikurangi harus lebih dari 0.");
            }

            if (item == null)
            {
                throw new InvalidOperationException($"Stok tidak mencukupi. Stok tersedia: {item}");
            }

            if (item.Stok <= item.BatasMinimum)
            {
                AutoRestock(item);
            }

            inventory.Remove(item);
            SaveInventory();
            Console.WriteLine($"Barang '{item.Nama}' berhasil dihapus.");


        }
        private void AutoRestock(InventoryItem item)
        {
            if (item.Stok <= item.BatasMinimum)
            {
                item.Stok += item.JumlahRestok;
                SaveInventory();

                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] Restok otomatis: Barang '{item.Nama}' ditambah {item.JumlahRestok} unit (Stok sekarang: {item.Stok})";
                Console.WriteLine(logMessage);
                File.AppendAllText("log.txt", logMessage + Environment.NewLine);
            }
        }

        public void ListItems()
        {
            Console.WriteLine("\n=== Daftar Barang ===");
            if (!inventory.Any())
            {
                Console.WriteLine("Tidak ada barang dalam inventory.");
                return;
            }

            foreach (var item in inventory)
            {
                Console.WriteLine($"ID: {item.Id} | Nama: {item.Nama} | Kategori: {item.Kategori} | Stok: {item.Stok} | Minimum: {item.BatasMinimum} | Restok: {item.JumlahRestok}");
            }
        }

        // Cek dan tampilkan barang yang perlu restok
        public void CheckRestockItems()
        {
            var needRestock = inventory.Where(i => i.Stok <= i.BatasMinimum).ToList();

            if (!needRestock.Any())
            {
                Console.WriteLine("Tidak ada barang yang perlu direstok.");
                return;
            }

            Console.WriteLine("\nBarang yang perlu direstok:");
            foreach (var item in needRestock)
            {
                Console.WriteLine($"ID: {item.Id} | Nama: {item.Nama} | Stok: {item.Stok} | Minimum: {item.BatasMinimum} | Jumlah Restok: {item.JumlahRestok}");
            }
        }
    }
}

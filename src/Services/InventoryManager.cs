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

<<<<<<< HEAD
        // Tambah Barang
=======
        // ✅ Tambah Barang
        #region UpdateByID    
>>>>>>> 303bebb9757e19753a5ddae0930922d1157b1283
        public void AddItem()
        {
            if (inventory == null)
            {
                inventory = new List<InventoryItem>();
            }

            Console.Write("Nama Barang: ");
            var nama = Console.ReadLine();
            if (string.IsNullOrEmpty(nama))
            {
<<<<<<< HEAD
                Console.WriteLine("Stok harus berupa angka positif.");
=======
                Console.WriteLine("❌ Nama barang tidak boleh kosong.");
>>>>>>> 303bebb9757e19753a5ddae0930922d1157b1283
                return;
            }

            var itemExists = inventory.FirstOrDefault(i => i.Nama?.Equals(nama, StringComparison.OrdinalIgnoreCase) == true);
            if (itemExists != null)
            {
                Console.WriteLine("❌ Barang dengan nama yang sama sudah ada.");
                return;
            }

            Console.Write("Kategori Barang: ");
            var kategori = Console.ReadLine();
            Console.Write("Jumlah Stok: ");
            if (!int.TryParse(Console.ReadLine(), out int stok))
            {
                Console.WriteLine("❌ Jumlah stok harus berupa angka.");
                return;
            }
            Console.Write("Batas Minimum Stok: ");
            if (!int.TryParse(Console.ReadLine(), out int batasMinimum))
            {
<<<<<<< HEAD
                Console.WriteLine("Batas minimum harus berupa angka positif.");
=======
                Console.WriteLine("❌ Batas minimum stok harus berupa angka.");
>>>>>>> 303bebb9757e19753a5ddae0930922d1157b1283
                return;
            }

            inventory.Add(new InventoryItem
            {
<<<<<<< HEAD
                Console.WriteLine("Jumlah restok harus berupa angka positif.");
                return;
            }

            var newItem = new InventoryItem
            {
                Id = inventory.Count > 0 ? inventory.Max(i => i.Id) + 1 : 1,
=======
                Id = inventory.Count + 1,
>>>>>>> 303bebb9757e19753a5ddae0930922d1157b1283
                Nama = nama,
                Kategori = kategori,
                Stok = stok,
                BatasMinimum = batasMinimum
            });

            SaveInventory();
<<<<<<< HEAD
            Console.WriteLine("Barang berhasil ditambahkan!");
=======
            Console.WriteLine($"✅ Barang {nama} berhasil ditambahkan!");
        }


        #endregion

        private int GetPositiveNumber(string prompt)
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out int result) || result < 0)
            {
                Console.WriteLine("❌ Masukkan angka positif.");
                return -1;
            }
            return result;
>>>>>>> 303bebb9757e19753a5ddae0930922d1157b1283
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
<<<<<<< HEAD
            Console.WriteLine("Barang berhasil diperbarui!");
=======
            Console.WriteLine($"✅ Barang dengan ID {id} berhasil diperbarui!");
>>>>>>> 303bebb9757e19753a5ddae0930922d1157b1283
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

            inventory.Remove(item);
            SaveInventory();
            Console.WriteLine($"✅ Barang dengan ID {id} berhasil dihapus!");
        }

        // ✅ Lihat Daftar Barang
        public void ListItems()
        {
            Console.WriteLine("\n=== DAFTAR BARANG ===");
            if (inventory.Count == 0)
            {
                Console.WriteLine("⚠️ Tidak ada barang dalam daftar.");
                return;
            }

            foreach (var item in inventory)
            {
                Console.WriteLine($"[{item.Id}] {item.Nama} (Kategori: {item.Kategori}, Stok: {item.Stok})");
            }
        }

        // ✅ Barang Perlu Restok
        public void CheckRestockItems()
        {
            var restockItems = inventory.Where(i => i.Stok <= i.BatasMinimum).ToList();
            if (!restockItems.Any())
            {
                Console.WriteLine("⚠️ Tidak ada barang yang perlu restok.");
                return;
            }

            Console.WriteLine("\n=== BARANG YANG PERLU RESTOK ===");
            foreach (var item in restockItems)
            {
                Console.WriteLine($"[{item.Id}] {item.Nama} (Kategori: {item.Kategori}, Stok: {item.Stok})");
            }
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
<<<<<<< HEAD

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

        // 🔔 Cek dan tampilkan barang yang perlu restok
        public void CheckRestockItems()
        {
            var needRestock = inventory.Where(i => i.Stok <= i.BatasMinimum).ToList();

            if (!needRestock.Any())
            {
                Console.WriteLine("Tidak ada barang yang perlu direstok.");
                return;
            }

            Console.WriteLine("\n Barang yang perlu direstok:");
            foreach (var item in needRestock)
            {
                Console.WriteLine($"ID: {item.Id} | Nama: {item.Nama} | Stok: {item.Stok} | Minimum: {item.BatasMinimum} | Jumlah Restok: {item.JumlahRestok}");
            }
        }
=======
>>>>>>> 303bebb9757e19753a5ddae0930922d1157b1283
    }
}

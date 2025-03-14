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
        public InventoryItem? GetItemById(int id)
        {
            return inventory.FirstOrDefault(i => i.Id == id);
        }

        private void LoadInventory()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File inventory.json tidak ditemukan.");
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);

                // Deserialisasi dengan opsi agar case-insensitive
                inventory = JsonSerializer.Deserialize<List<InventoryItem>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<InventoryItem>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Gagal memuat inventory.json: {ex.Message}");
                inventory = new List<InventoryItem>(); // Supaya program tetap berjalan meskipun JSON error
            }
        }

       private void SaveInventory()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonData = JsonSerializer.Serialize(inventory, options);
            File.WriteAllText(filePath, jsonData);
        }

        public InventoryItem? GetItemById(int id)
        {
            return inventory.FirstOrDefault(i => i.Id == id);
        }

        // ✅ Tambah Barang
        #region UpdateByID    
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
                Console.WriteLine("❌ Nama barang tidak boleh kosong.");
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
                Console.WriteLine("❌ Batas minimum stok harus berupa angka.");
                return;
            }

            inventory.Add(new InventoryItem
            {
                Id = inventory.Count + 1,
                Nama = nama,
                Kategori = kategori,
                Stok = stok,
                BatasMinimum = batasMinimum
            });

            SaveInventory();
            Console.WriteLine($"✅ Barang {nama} berhasil ditambahkan!");
        }


        #endregion
        public void SelectItemForAutoRestock()
        {
            Console.WriteLine("\n=== PILIH BARANG UNTUK AUTO RESTOCK ===");

            if (inventory.Count == 0)
            {
                Console.WriteLine("⚠️ Tidak ada barang dalam daftar.");
                return;
            }

            // Tampilkan daftar barang
            foreach (var inventoryItem in inventory) // Ubah nama variabel dari `item` ke `inventoryItem`
            {
                Console.WriteLine($"[{inventoryItem.Id}] {inventoryItem.Nama} (Kategori: {inventoryItem.Kategori}, Stok: {inventoryItem.Stok}, Batas Minimum: {inventoryItem.BatasMinimum}, Jumlah Restok: {inventoryItem.JumlahRestok})");
            }

            Console.Write("Masukkan ID barang yang ingin di-auto restock: ");
            if (!int.TryParse(Console.ReadLine(), out int itemId))
            {
                Console.WriteLine("❌ ID tidak valid.");
                return;
            }

            // Cari barang berdasarkan ID
            var selectedItem = inventory.FirstOrDefault(i => i.Id == itemId); // Ubah nama variabel dari `item` ke `selectedItem`
            if (selectedItem == null)
            {
                Console.WriteLine("❌ Barang tidak ditemukan.");
                return;
            }

            // Cek apakah barang memenuhi syarat auto restock
            if (selectedItem.Stok > selectedItem.BatasMinimum)
            {
                Console.WriteLine($"⚠️ Barang '{selectedItem.Nama}' tidak membutuhkan restok (Stok saat ini: {selectedItem.Stok}, Batas Minimum: {selectedItem.BatasMinimum}).");
                return;
            }

            // Lakukan restok otomatis
            AutoRestock(selectedItem);
            Console.WriteLine($"✅ Barang '{selectedItem.Nama}' berhasil di-auto restock.");
        }

        private int GetPositiveNumber(string prompt)
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out int result) || result < 0)
            {
                Console.WriteLine("❌ Masukkan angka positif.");
                return -1;
            }
            return result;
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
            Console.WriteLine($"✅ Barang dengan ID {id} berhasil diperbarui!");
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

        //mengurangi barang 
        public void DecreaseItem(int id)
        {
            var item = inventory.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("❌ Barang tidak ditemukan.");
                return;
            }

            Console.Write($"Jumlah yang akan dikurangi dari stok {item.Nama} (Stok saat ini: {item.Stok}): ");
            if (!int.TryParse(Console.ReadLine(), out int jumlahPengurangan) || jumlahPengurangan <= 0)
            {
                Console.WriteLine("❌ Masukkan jumlah yang valid dan lebih besar dari nol.");
                return;
            }

            if (item.Stok < jumlahPengurangan)
            {
                Console.WriteLine($"❌ Stok tidak mencukupi. Hanya tersedia {item.Stok} unit.");
                return;
            }

            item.Stok -= jumlahPengurangan;
            SaveInventory();

            Console.WriteLine($"✅ Stok {item.Nama} berhasil dikurangi sebanyak {jumlahPengurangan}. Stok saat ini: {item.Stok}");

            // Auto restock jika perlu
            if (item.Stok <= item.BatasMinimum)
            {
                AutoRestock(item);
            }
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
        public void AutoRestock(InventoryItem item)
        {
            item.Stok += item.JumlahRestok;
            SaveInventory();

            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] Restok otomatis: Barang '{item.Nama}' ditambah {item.JumlahRestok} unit (Stok sekarang: {item.Stok})";
            Console.WriteLine(logMessage);
            File.AppendAllText("log.txt", logMessage + Environment.NewLine);
        }
    }
}


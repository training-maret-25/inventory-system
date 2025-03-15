using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public class TransactionManager
    {
        private readonly string transactionFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "transaction.json");
        private List<Transaction> transactions = new();

        public TransactionManager()
        {
            LoadTransactions();
        }

        public void LoadTransactions()
        {
            string path = @"C:\conen\kuliah\magang\maret\inventory-system\data\inventory.json";

            if (!File.Exists(path))
            {
                Console.WriteLine("File inventory.json tidak ditemukan!");
                return;
            }

            try
            {
                string jsonData = File.ReadAllText(path).Trim();

                if (string.IsNullOrEmpty(jsonData))
                {
                    Console.WriteLine("File kosong, menginisialisasi dengan array kosong.");
                    jsonData = "[]"; 
                }

                var data = JsonSerializer.Deserialize<List<InventoryItem>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data == null || !data.Any())
                {
                    Console.WriteLine("Data inventory kosong atau tidak valid!");
                    return;
                }

                Console.WriteLine("Data inventory berhasil dimuat!");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Kesalahan saat membaca JSON: {ex.Message}");
            }
        }

        private void SaveTransactions()
        {
            string jsonData = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(transactionFilePath, jsonData);
        }

        public void RecordTransaction(string jenis, int barangId, string namaBarang, int jumlah, int stokSetelah, string employee)
        {
            Console.Write("Masukkan tanggal transaksi (yyyy-MM-dd): ");
            string inputTanggal = Console.ReadLine() ?? "";

            if (DateTime.TryParse(inputTanggal, out DateTime tanggal))
            {
                int newId = transactions.Any() ? transactions.Max(t => t.Id) + 1 : 1; // Auto-increment ID

                Transaction transaksi = new Transaction
                {
                    Id = newId,
                    Tanggal = tanggal,
                    Jenis = jenis,
                    BarangId = barangId,
                    NamaBarang = namaBarang,
                    Jumlah = jumlah,
                    StokSetelah = stokSetelah,
                    Employee = employee
                };

                transactions.Add(transaksi);
                SaveTransactions();

                Console.WriteLine($"Transaksi berhasil dicatat pada {transaksi.Tanggal}");
            }
            else
            {
                Console.WriteLine("Format tanggal tidak valid!");
            }
        }

        public void ListTransactions()
        {
            Console.WriteLine("\n=== Riwayat Transaksi ===");
            if (!transactions.Any())
            {
                Console.WriteLine("Tidak ada transaksi yang tercatat.");
                return;
            }

            foreach (var trans in transactions)
            {
                Console.WriteLine($"ID: {trans.Id} | Tanggal: {trans.Tanggal:yyyy-MM-dd} | Jenis: {trans.Jenis} | Barang: {trans.NamaBarang} | Jumlah: {trans.Jumlah} | Stok Setelah: {trans.StokSetelah} | Employee: {trans.Employee}");
            }
        }

        public void AddTransactions()
        {
            Console.WriteLine("\n=== Tambah Transaksi ===");
            Console.Write("Masukkan jenis transaksi (Masuk/Keluar): ");
            string jenis = Console.ReadLine() ?? "";

            Console.Write("Masukkan ID Barang: ");
            if (!int.TryParse(Console.ReadLine(), out int barangId))
            {
                Console.WriteLine("ID Barang tidak valid!");
                return;
            }

            Console.Write("Masukkan Nama Barang: ");
            string namaBarang = Console.ReadLine() ?? "";

            Console.Write("Masukkan Jumlah: ");
            if (!int.TryParse(Console.ReadLine(), out int jumlah))
            {
                Console.WriteLine("Jumlah tidak valid!");
                return;
            }

            Console.Write("Masukkan Stok Setelah Transaksi: ");
            if (!int.TryParse(Console.ReadLine(), out int stokSetelah))
            {
                Console.WriteLine("Stok tidak valid!");
                return;
            }

            Console.Write("Masukkan Nama Pegawai: ");
            string employee = Console.ReadLine() ?? "";

            RecordTransaction(jenis, barangId, namaBarang, jumlah, stokSetelah, employee);
        }

        public void ViewTransactionHistory()
        {
            ListTransactions();
        }

        public void GenerateReport()
        {
            Console.WriteLine("\n=== Laporan Rekapitulasi Transaksi ===");
            if (!transactions.Any())
            {
                Console.WriteLine("Tidak ada transaksi yang tercatat.");
                return;
            }

            var report = transactions
                .GroupBy(t => new { t.Jenis, t.NamaBarang })
                .Select(g => new
                {
                    Jenis = g.Key.Jenis,
                    NamaBarang = g.Key.NamaBarang,
                    TotalTransaksi = g.Count(),
                    TotalBarang = g.Sum(t => t.Jumlah),
                    StokAkhir = g.Last().StokSetelah 
                });

            foreach (var item in report)
            {
                Console.WriteLine($"Jenis: {item.Jenis} | Barang: {item.NamaBarang} | Total Transaksi: {item.TotalTransaksi} | Total Barang: {item.TotalBarang} | Stok Akhir: {item.StokAkhir}");
            }
        }

        public void ExportReport()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "report.txt");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("=== Laporan Rekapitulasi Transaksi ===");

                var report = transactions
                    .GroupBy(t => new { t.Jenis, t.NamaBarang })
                    .Select(g => new
                    {
                        Jenis = g.Key.Jenis,
                        NamaBarang = g.Key.NamaBarang,
                        TotalTransaksi = g.Count(),
                        TotalBarang = g.Sum(t => t.Jumlah),
                        StokAkhir = g.Last().StokSetelah
                    });

                foreach (var item in report)
                {
                    writer.WriteLine($"Jenis: {item.Jenis} | Barang: {item.NamaBarang} | Total Transaksi: {item.TotalTransaksi} | Total Barang: {item.TotalBarang} | Stok Akhir: {item.StokAkhir}");
                }
            }

            Console.WriteLine($"Laporan berhasil diekspor ke {filePath}");
        }
    }
}

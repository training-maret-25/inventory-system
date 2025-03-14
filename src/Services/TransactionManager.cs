using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public class TransactionManager
    {
        private readonly string jsonFilePath = Path.Combine("data", "transaksion.json");
        private readonly string reportFilePath = Path.Combine("data", "report.txt");
        private readonly InventoryManager inventoryManager;

        public TransactionManager(InventoryManager inventoryManager)
        {
            this.inventoryManager = inventoryManager;
        }


        private List<Transaction> transactions = new();

        public void RecordTransaction(int barangId, string namaBarang, int jumlah, string jenis)
        {
            var transaction = new Transaction
            {
                BarangId =  barangId,
                NamaBarang = namaBarang,
                Jumlah = jumlah,
                Jenis = jenis,
                Tanggal = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            transactions.Add(transaction);
            SaveTransactionsToJson();
            SaveTransactionsToText();
        }

        public void SaveTransactionsToJson()
        {
            string jsonData = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonFilePath, jsonData);
            Console.WriteLine("✅ Transaksi disimpan ke transaksi.json");
        }

        public void SaveTransactionsToText()
        {
            List<string> reportLines = new()
            {
                "=== LAPORAN TRANSAKSI ===",
                $"Tanggal: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                "---------------------------------"
            };

            foreach (var transaksi in transactions)
            {
                reportLines.Add($"ID: {transaksi.BarangId}, Nama: {transaksi.NamaBarang}, Jumlah: {transaksi.Jumlah}, Jenis: {transaksi.Jenis}, Tanggal: {transaksi.Tanggal}");
            }

            File.WriteAllLines(reportFilePath, reportLines);
            Console.WriteLine("✅ Laporan transaksi disimpan ke report.txt");
        }
    }
}

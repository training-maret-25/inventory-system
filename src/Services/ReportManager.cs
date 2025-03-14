using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using InventorySystem.Models;
using Newtonsoft.Json;

namespace InventorySystem.Services
{
    public class ReportManager
    {
        private const string TransactionFile = "data/transaction.json";
        private const string ReportFile = "data/report.txt";

        // Model transaksi
        public class Transaction
        {
            public int Id { get; set; }
            public string NamaBarang { get; set; }
            public string JenisTransaksi { get; set; }             
            public int Jumlah { get; set; }
            public DateTime Tanggal { get; set; }
            public string Employee { get; set; }
        }

        // Load semua transaksi
        private List<Transaction> LoadTransactions()
        {
            if (!File.Exists(TransactionFile)) return new List<Transaction>();

            string json = File.ReadAllText(TransactionFile);
            return JsonConvert.DeserializeObject<List<Transaction>>(json) ?? new List<Transaction>();
        }

        // === 1. Laporan Harian ===
        public void GenerateDailyReport(string tanggal)
        {
            var transactions = LoadTransactions();

            if (!DateTime.TryParseExact(tanggal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime targetDate))
            {
                Console.WriteLine("Format tanggal salah. Gunakan yyyy-MM-dd.");
                return;
            }

            var dailyTransactions = transactions.Where(t => t.Tanggal.Date == targetDate.Date).ToList();

            List<string> reportLines = new List<string>
            {
                $"=== LAPORAN HARIAN TANGGAL {tanggal} ===",
                "-----------------------------------------------------"
            };

            int totalMasuk = 0, totalKeluar = 0;

            foreach (var t in dailyTransactions)
            {
                reportLines.Add($"{t.Tanggal:yyyy-MM-dd} | {t.NamaBarang} | {t.JenisTransaksi} | {t.Jumlah} | Oleh: {t.Employee}");
                if (t.JenisTransaksi == "Masuk") totalMasuk += t.Jumlah;
                else if (t.JenisTransaksi == "Keluar") totalKeluar += t.Jumlah;
            }

            reportLines.Add("-----------------------------------------------------");
            reportLines.Add($"Total Barang Masuk : {totalMasuk}");
            reportLines.Add($"Total Barang Keluar: {totalKeluar}");
            reportLines.Add("\n");

            File.AppendAllLines(ReportFile, reportLines);
        }

        // === 2. Laporan Bulanan ===
        public void GenerateMonthlyReport(string bulan)
        {
            var transactions = LoadTransactions();

            if (!DateTime.TryParseExact(bulan + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime targetMonth))
            {
                Console.WriteLine("Format bulan salah. Gunakan yyyy-MM.");
                return;
            }

            var monthlyTransactions = transactions.Where(t => t.Tanggal.Year == targetMonth.Year && t.Tanggal.Month == targetMonth.Month).ToList();

            List<string> reportLines = new List<string>
            {
                $"=== LAPORAN BULANAN BULAN {bulan} ===",
                "-----------------------------------------------------"
            };

            int totalMasuk = 0, totalKeluar = 0;

            foreach (var t in monthlyTransactions)
            {
                reportLines.Add($"{t.Tanggal:yyyy-MM-dd} | {t.NamaBarang} | {t.JenisTransaksi} | {t.Jumlah} | Oleh: {t.Employee}");
                if (t.JenisTransaksi == "Masuk") totalMasuk += t.Jumlah;
                else if (t.JenisTransaksi == "Keluar") totalKeluar += t.Jumlah;
            }

            reportLines.Add("-----------------------------------------------------");
            reportLines.Add($"Total Barang Masuk : {totalMasuk}");
            reportLines.Add($"Total Barang Keluar: {totalKeluar}");
            reportLines.Add("\n");

            File.AppendAllLines(ReportFile, reportLines);
        }

        // === 3. Laporan Per Barang (Harian atau Bulanan) ===
        public void GenerateItemReport(string namaBarang, string periode)
        {
            var transactions = LoadTransactions();

            DateTime? targetDate = null;
            bool isMonthly = false;

            if (DateTime.TryParseExact(periode, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                targetDate = parsedDate;
            }
            else if (DateTime.TryParseExact(periode + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedMonth))
            {
                targetDate = parsedMonth;
                isMonthly = true;
            }
            else
            {
                Console.WriteLine("Format periode salah. Gunakan yyyy-MM-dd atau yyyy-MM.");
                return;
            }

            var itemTransactions = transactions.Where(t =>
                t.NamaBarang.Equals(namaBarang, StringComparison.OrdinalIgnoreCase) &&
                (isMonthly
                    ? t.Tanggal.Year == targetDate.Value.Year && t.Tanggal.Month == targetDate.Value.Month
                    : t.Tanggal.Date == targetDate.Value.Date)
            ).ToList();

            List<string> reportLines = new List<string>
            {
                $"=== LAPORAN { (isMonthly ? "BULANAN" : "HARIAN") } UNTUK BARANG: {namaBarang.ToUpper()} ({periode}) ===",
                "-----------------------------------------------------"
            };

            int totalMasuk = 0, totalKeluar = 0;

            foreach (var t in itemTransactions)
            {
                reportLines.Add($"{t.Tanggal:yyyy-MM-dd} | {t.JenisTransaksi} | {t.Jumlah} | Oleh: {t.Employee}");
                if (t.JenisTransaksi == "Masuk") totalMasuk += t.Jumlah;
                else if (t.JenisTransaksi == "Keluar") totalKeluar += t.Jumlah;
            }

            reportLines.Add("-----------------------------------------------------");
            reportLines.Add($"Total Masuk : {totalMasuk}");
            reportLines.Add($"Total Keluar: {totalKeluar}");
            reportLines.Add("\n");

            File.AppendAllLines(ReportFile, reportLines);
        }
    }
}

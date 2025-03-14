using InventorySystem.Models;
using System.Text.Json;

namespace InventorySystem.Services
{
    public class TransactionManager
    {
        private readonly string transactionFile = "data/transaction.json";
        private readonly string reportFile = "data/report.txt";

        // Generate Laporan Harian
        public void GenerateDailyReport(DateTime date)
        {
            var transactions = LoadTransactions();
            var dailyTransactions = transactions.Where(t => t.Tanggal.Date == date.Date).ToList();

            if (!dailyTransactions.Any())
            {
                Console.WriteLine("❌ Tidak ada transaksi pada tanggal tersebut.");
                return;
            }

            var barangMasuk = new List<string>();
            var barangKeluar = new List<string>();
            int totalPerubahan = 0;

            foreach (var trans in dailyTransactions)
            {
                if (string.IsNullOrWhiteSpace(trans.NamaBarang)) continue; // Skip jika nama kosong

                string item = $"{trans.Jumlah} {trans.NamaBarang}";
                if (trans.Jenis == "Barang Masuk")
                {
                    barangMasuk.Add(item);
                    totalPerubahan += trans.Jumlah;
                }
                else if (trans.Jenis == "Barang Keluar")
                {
                    barangKeluar.Add(item);
                    totalPerubahan -= trans.Jumlah;
                }
            }

            // Format laporan
            string reportContent = $"\n[{date:yyyy-MM-dd}] Laporan Harian:\n";
            reportContent += $"- Barang Masuk: {(barangMasuk.Count > 0 ? string.Join(", ", barangMasuk) : "-")}\n";
            reportContent += $"- Barang Keluar: {(barangKeluar.Count > 0 ? string.Join(", ", barangKeluar) : "-")}\n";
            reportContent += $"Total Perubahan Stok: {(totalPerubahan >= 0 ? "+" : "")}{totalPerubahan}\n";

            // Simpan ke file
            File.AppendAllText(reportFile, reportContent);
            Console.WriteLine("✅ Laporan harian berhasil dibuat.");
        }

        // Generate Laporan Bulanan
        public void GenerateMonthlyReport(int year, int month)
        {
            var transactions = LoadTransactions();
            var monthlyTransactions = transactions.Where(t => t.Tanggal.Year == year && t.Tanggal.Month == month).ToList();

            if (!monthlyTransactions.Any())
            {
                Console.WriteLine("❌ Tidak ada transaksi pada bulan tersebut.");
                return;
            }

            var summary = new Dictionary<string, (int masuk, int keluar)>();

            foreach (var trans in monthlyTransactions)
            {
                if (string.IsNullOrWhiteSpace(trans.NamaBarang)) continue; // Skip jika nama kosong

                // Tambahkan key baru jika belum ada
                if (!summary.ContainsKey(trans.NamaBarang))
                    summary[trans.NamaBarang] = (0, 0);

                // Update jumlah masuk/keluar
                if (trans.Jenis == "Barang Masuk")
                    summary[trans.NamaBarang] = (summary[trans.NamaBarang].masuk + trans.Jumlah, summary[trans.NamaBarang].keluar);
                else if (trans.Jenis == "Barang Keluar")
                    summary[trans.NamaBarang] = (summary[trans.NamaBarang].masuk, summary[trans.NamaBarang].keluar + trans.Jumlah);
            }

            int totalPerubahan = summary.Sum(s => s.Value.masuk - s.Value.keluar);

            // Format laporan
            string reportContent = $"\n[{year}-{month:D2}] Laporan Bulanan:\n";
            foreach (var item in summary)
            {
                reportContent += $"- {item.Key}: +{item.Value.masuk} masuk, -{item.Value.keluar} keluar\n";
            }
            reportContent += $"Total Perubahan Stok: {(totalPerubahan >= 0 ? "+" : "")}{totalPerubahan}\n";

            // Simpan ke file
            File.AppendAllText(reportFile, reportContent);
            Console.WriteLine("✅ Laporan bulanan berhasil dibuat.");
        }

        // Load data transaksi dari JSON
        private List<Transaction> LoadTransactions()
        {
            if (!File.Exists(transactionFile))
                return new List<Transaction>();

            string json = File.ReadAllText(transactionFile);
            return JsonSerializer.Deserialize<List<Transaction>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Transaction>();
        }
    }
}

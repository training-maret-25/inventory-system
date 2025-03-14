using InventorySystem.Models;
using System.Text.Json;

namespace InventorySystem.Services
{
    public class TransactionManager
    {
        private readonly string transactionFile = "data/transaction.json";
        private readonly string reportFile = "data/report.txt";
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss";

        // Generate Laporan Harian
        public void GenerateDailyReport(DateTime date)
        {
            var transactions = LoadTransactions();
            var dailyTransactions = transactions
                .Where(t => DateTime.ParseExact(t.Tanggal, DateFormat, null).Date == date.Date)
                .ToList();

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

            // Simpan ke file report.txt
            File.AppendAllText(reportFile, reportContent);
            Console.WriteLine("✅ Laporan harian berhasil dibuat.");
        }

        // Generate Laporan Bulanan
        public void GenerateMonthlyReport(int year, int month)
        {
            var transactions = LoadTransactions();
            var monthlyTransactions = transactions
                .Where(t => DateTime.ParseExact(t.Tanggal, DateFormat, null).Year == year &&
                            DateTime.ParseExact(t.Tanggal, DateFormat, null).Month == month)
                .ToList();

            if (!monthlyTransactions.Any())
            {
                Console.WriteLine("❌ Tidak ada transaksi pada bulan tersebut.");
                return;
            }

            var summary = new Dictionary<string, (int masuk, int keluar)>();

            foreach (var trans in monthlyTransactions)
            {
                if (!summary.ContainsKey(trans.NamaBarang))
                    summary[trans.NamaBarang] = (0, 0);

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

            // Simpan ke file report.txt
            File.AppendAllText(reportFile, reportContent);
            Console.WriteLine("✅ Laporan bulanan berhasil dibuat.");
        }

        // Load JSON Transactions
        private List<Transaction> LoadTransactions()
        {
            if (!File.Exists(transactionFile))
            {
                Console.WriteLine("⚠️ File transaction.json tidak ditemukan!");
                return new List<Transaction>();
            }

            string json = File.ReadAllText(transactionFile);
            var result = JsonSerializer.Deserialize<List<Transaction>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null || result.Count == 0)
                Console.WriteLine("⚠️ File transaction.json kosong atau format salah!");

            return result ?? new List<Transaction>();
        }
    }
}

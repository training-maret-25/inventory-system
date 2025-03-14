using System;
using System.IO;

namespace logger
{
    public static class Logger
    {
        private static readonly string LogFilePath = "data/log.txt";

        // function untuk menambahkan messege ke file log.txt
        private static void WriteToFile(string logEntry) {
            try {
                using (StreamWriter writer = new StreamWriter(LogFilePath, true)) {
                    writer.WriteLine(logEntry);
                }
            } catch (Exception ex) {
                string errorLogPath = "data/error_log.txt";
                try {
                    using (StreamWriter writer = new StreamWriter(errorLogPath, true)) {
                        writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] Gagal menulis log utama: {ex.Message}");
                    }
                } catch {
                    Console.WriteLine("Fatal: gagal menulis ke log file!");
                }
            }
        }

        public static void LogCriticalError (string username, string errorMessage) {
            Log("CRITICAL", username, errorMessage);
        }

        public static void LogException(string username, Exception ex) {
            Log("EXCEPTION", username, $"Exception terjadi: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }

        // fungsi untuk mencatat log
        public static void Log(string level, string username, string action) {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] User '{username} melakukan aksi '{action}'";
            Console.WriteLine(logEntry);
            WriteToFile(logEntry);
        }

        // function untuk mencatat error
        public static void LogError(string username, string errorMessage) {
            Log("ERROR", username, errorMessage);
        }

        // function untuk mencatat login/logout
        public static void LogLogin( string username) {
            Log("INFO", username, "Login ke sistem");
        }

        public static void LogLogout(string username) {
            Log("INFO", username, "Logout dari sistem");
        }

        // function untuk mencatat perubahan user
        public static void LogUserModification(string username, string action) {
            Log("USER", username, action);
        }

        // Fuction untuk mencatat perubahan barang 
        public static void LogInventoryChange (string username, string action) {
            Log("INVENTORY", username, action);
        }

        // Function untuk mencatat transaksi barang
        public static void LogTransaction(string username, string action) {
            Log("TRANSACTION", username, action);
        }

        // fuction untuk mencatat restock otomatis
        public static void LogRestock(string username, string itemName, int amount) {
            Log("RESTOCK", username, $"Restock otomatis: {amount} unit '{itemName}' ditambahkan");
        }
    }
}
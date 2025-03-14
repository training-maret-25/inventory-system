using System;
using System.IO;

namespace logger
{
    public static class Logger
    {
        private static readonly string LogFilePath = "data/log.txt";
        private static readonly string ErrorFilePath = "data/error_log.txt";

        // function untuk menambahkan messege ke file log.txt
        private static void WriteToFile(string logEntry) {
            try {
                string directory = Path.GetDirectoryName(LogFilePath);
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }

                using (StreamWriter writer = new StreamWriter(LogFilePath, true)) {
                    writer.WriteLine(logEntry);
                }
            } catch (Exception ex) {
                try {
                    string directory = Path.GetDirectoryName(ErrorFilePath);
                    if (!Directory.Exists(directory)) {
                        Directory.CreateDirectory(directory);
                    }
                    using (StreamWriter writer = new StreamWriter(ErrorFilePath, true)) {
                        writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] Gagal menulis log utama: {ex.Message}");
                    }
                } catch {
                    Console.WriteLine("Fatal: gagal menulis ke log file!");
                }
            }
        }

        public static void LogException(string username, Exception ex) {
            Log("EXCEPTION", username, $"Exception terjadi: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }

        public static void LogWarning(string username, string WarningMessage) {
            Log("WARNING", username, WarningMessage);
        }

        public static void LogInfo(string username, string action) {
            Log("INFO", username, action);
        }

        public static void LogError(string username, string errorMessage) {
            Log("ERROR", username, errorMessage);
        }

        // fungsi untuk mencatat log
        public static void Log(string level, string username, string action) {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] User '{username} melakukan aksi '{action}'";
            Console.WriteLine(logEntry);
            WriteToFile(logEntry);
        }

        // fuction untuk mencatat restock otomatis
        public static void LogRestock(string username, string itemName, int amount) {
            Log("INFO", username, $"Restock otomatis: {amount} unit '{itemName}' ditambahkan");
        }
    }
}
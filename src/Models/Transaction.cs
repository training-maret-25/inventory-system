using System;

namespace InventorySystem.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Tanggal { get; set; } = string.Empty;  // Default value
        public string Jenis { get; set; } = string.Empty;    // Default value
        public int BarangId { get; set; }
        public string NamaBarang { get; set; } = string.Empty; // Default value
        public int Jumlah { get; set; }
        public int StokSetelah { get; set; }
        public string Employee { get; set; } = "Unknown";  // Default value
    }
}

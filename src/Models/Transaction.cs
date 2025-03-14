using System;

namespace InventorySystem.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Tanggal { get; set; }
        public string Jenis { get; set; } // "Barang Masuk" atau "Barang Keluar"
        public int BarangId { get; set; }
        public string NamaBarang { get; set; }
        public int Jumlah { get; set; }
        public int StokSetelah { get; set; }
        public string Employee { get; set; }
  }
}

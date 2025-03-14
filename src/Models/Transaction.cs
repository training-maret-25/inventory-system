using System.Text.Json.Serialization;

namespace InventorySystem.Models
{
    public class Transaction
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("tanggal")]
        public DateTime Tanggal { get; set; }

        [JsonPropertyName("jenis")]
        public string Jenis { get; set; }

        [JsonPropertyName("barang_id")]
        public int BarangId { get; set; }

        [JsonPropertyName("nama_barang")]
        public string NamaBarang { get; set; } = ""; // default kosong biar gak null

        [JsonPropertyName("jumlah")]
        public int Jumlah { get; set; }

        [JsonPropertyName("stok_setelah")]
        public int StokSetelah { get; set; }

        [JsonPropertyName("employee")]
        public string Employee { get; set; }
    }
}

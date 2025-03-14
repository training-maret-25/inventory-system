using System.Text.Json.Serialization;

namespace InventorySystem.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }

        [JsonPropertyName("nama")]
        public string? Nama { get; set; }

        [JsonPropertyName("kategori")]
        public string? Kategori { get; set; }

        public int Stok { get; set; }

        [JsonPropertyName("batas_minimum")]
        public int BatasMinimum { get; set; } = 10;

        [JsonPropertyName("jumlah_restok")]
        public int JumlahRestok { get; set; }
    }
}

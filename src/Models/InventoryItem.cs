namespace InventorySystem.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public required string Nama { get; set; }
        public required string Kategori { get; set; }
        public int Stok { get; set; }
        public int BatasMinimum { get; set; }
        public int JumlahRestok { get; set; }
    }
}

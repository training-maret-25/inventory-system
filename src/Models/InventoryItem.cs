namespace InventorySystem.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Kategori { get; set; }
        public int Stok { get; set; }
        public int BatasMinimum { get; set; } = 10;
        public int JumlahRestok { get; set; }
    }
}

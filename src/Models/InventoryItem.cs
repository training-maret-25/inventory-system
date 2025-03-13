namespace InventorySystem.Models;
class Barang
{
    public int Id { get; set; }
    public string Nama { get; set; }
    public string Kategori { get; set; }
    public int Stok { get; set; }
    public int BatasMinimum { get; set; }
    public int JumlahRestok { get; set; }
}


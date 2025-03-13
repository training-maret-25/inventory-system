using System;

public class Item
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public int Stock { get; set; }
  public int RestockThreshold { get; set; }
  public int RestockAmount { get; set; }

  public Item(int id, string name, int stock, int restockThreshold, int restockAmount)
  {
    Id = id;
    Name = name;
    Stock = stock;
    RestockThreshold = restockThreshold;
    RestockAmount = restockAmount;
  }

  public void AddStock(int quantity)
  {
    if (quantity <= 0)
    {
      throw new ArgumentException("Jumlah yang ditambahkan harus lebih dari 0.");
    }

    Stock += quantity;
    Console.WriteLine($"✅ Stok untuk barang '{Name}' berhasil ditambahkan sebanyak {quantity}. (Stok sekarang: {Stock})");
  }

  public void ReduceStock(int quantity)
  {
    if (quantity <= 0)
    {
      throw new ArgumentException("Jumlah yang dikurangi harus lebih dari 0.");
    }

    if (quantity > Stock)
    {
      throw new InvalidOperationException($"Stok tidak mencukupi. Stok tersedia: {Stock}");
    }

    Stock -= quantity;
    Console.WriteLine($"✅ Stok untuk barang '{Name}' berhasil dikurangi sebanyak {quantity}. (Stok sekarang: {Stock})");

    if (Stock <= RestockThreshold)
    {
      AutoRestock();
    }
  }

  private void AutoRestock()
  {
    Stock += RestockAmount;
    string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] Restok otomatis: Barang '{Name}' ditambah {RestockAmount} unit (Stok sekarang: {Stock})";
    Console.WriteLine(logMessage);
    File.AppendAllText("log.txt", logMessage + Environment.NewLine);
  }
}

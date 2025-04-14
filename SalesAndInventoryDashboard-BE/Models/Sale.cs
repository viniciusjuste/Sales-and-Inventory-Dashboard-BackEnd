namespace SalesAndInventoryDashboard_BE.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public List<SaleItem> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}


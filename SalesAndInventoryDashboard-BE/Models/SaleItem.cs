using System.Text.Json.Serialization;

namespace SalesAndInventoryDashboard_BE.Models
{
    public class SaleItem
    {
        [JsonIgnore]
        public int SaleId { get; set; }
        [JsonIgnore]
        public Sale Sale { get; set; } = null!;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}


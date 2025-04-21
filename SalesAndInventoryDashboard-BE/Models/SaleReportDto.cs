    namespace SalesAndInventoryDashboard_BE.Models{
        public class SalesReportDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalSales { get; set; }
        public decimal AverageTicket { get; set; }
        public string BestSellingProduct { get; set; } = string.Empty;
        public int BestSellingProductQuantity { get; set; }
        public List<DailySaleDto> DailySales { get; set; } = new();
    }

    public class DailySaleDto
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }

    }
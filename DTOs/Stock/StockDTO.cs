using System.ComponentModel.DataAnnotations.Schema;

namespace StocksWebApi.DTOs.Stock
{
    public class StockDTO
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public decimal Divs { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Behoof.Infrastructure.Data
{
    public class HistoryProduct
    {
        public HistoryProduct()
        {
            Id = Guid.NewGuid().ToString();
            DateUpdate = DateTime.Now;
        }
        public string Id { get; set; }

        public Product? Product { get; set; }
        public string? ProductId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? DateUpdate { get; set; }
    }
}

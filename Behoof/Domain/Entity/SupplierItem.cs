using System.Text.Json.Serialization;
using Nancy.Json;

namespace Behoof.Domain.Entity
{
    public class SupplierItem
    {
        public SupplierItem() 
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public List<Product> Products { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? DateOfCreate { get; set; }
        public Supplier? Supplier { get; set; }
        public string? SupplierId { get; set; }
    }
}

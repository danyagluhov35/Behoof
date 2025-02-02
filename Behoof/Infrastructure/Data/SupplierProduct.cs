namespace Behoof.Infrastructure.Data
{
    public class SupplierProduct
    {
        public string? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public string? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}

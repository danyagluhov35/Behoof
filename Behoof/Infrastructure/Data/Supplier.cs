namespace Behoof.Infrastructure.Data
{
    public class Supplier
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public List<SupplierProduct>? SupplierProducts { get; set; }
    }
}

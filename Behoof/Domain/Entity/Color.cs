namespace Behoof.Domain.Entity
{
    public class Color
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public List<Product>? Product { get; set; }
    }
}

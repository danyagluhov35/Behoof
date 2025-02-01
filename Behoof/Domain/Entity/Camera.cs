namespace Behoof.Domain.Entity
{
    public class Camera
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public List<Product>? Product { get; set; }
    }
}

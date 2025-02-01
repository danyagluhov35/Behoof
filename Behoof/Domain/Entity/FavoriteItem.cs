namespace Behoof.Domain.Entity
{
    public class FavoriteItem
    {
        public FavoriteItem() 
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public Product? Product { get; set; }
        public string? ProductId { get; set; }
    }
}

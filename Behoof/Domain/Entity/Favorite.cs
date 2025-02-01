namespace Behoof.Domain.Entity
{
    public class Favorite
    {
        public Favorite() 
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public List<FavoriteItem>? FavoriteItem { get; set; }
        public User? User { get; set; }
    }
}

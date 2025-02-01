using System.ComponentModel.DataAnnotations;

namespace Behoof.Domain.Entity
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? Login { get; set; }
        public string? Passoword { get; set; }

        public Country? Country { get; set; }
        public string? CountryId { get; set; }

        public City? City { get; set; }
        public string? CityId { get; set; }

        public Favorite? Favorite { get; set; }
        public string? FavoriteId { get; set; }
    }
}

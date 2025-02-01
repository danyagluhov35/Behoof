namespace Behoof.Domain.Entity
{
    public class Country
    {
        public string Id { get; set; }
        public string? Name { get; set; }

        public List<User>? User { get; set; }
        public List<City>? City { get; set; }
    }
}

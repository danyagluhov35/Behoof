namespace Behoof.Domain.Entity
{
    public class City
    {
        public string Id { get; set; }
        public string? Name { get; set; }

        public Country? Country { get; set; }
        public string? CountryId { get; set; }
    }
}

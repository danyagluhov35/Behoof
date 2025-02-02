using Behoof.Infrastructure.Data;

namespace Behoof.Application.DTO
{
    public class UserUpdateDto
    {
        public string? Email { get; set; }
        public string? Login { get; set; }
        public string? Passoword { get; set; }

        public Country? Country { get; set; }
        public string? CountryId { get; set; }

        public City? City { get; set; }
        public string? CityId { get; set; }
    }
}

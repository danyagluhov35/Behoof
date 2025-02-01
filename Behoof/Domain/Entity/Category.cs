using System.Text.Json.Serialization;
using Nancy.Json;

namespace Behoof.Domain.Entity
{
    public class Category
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? ImageLink { get; set; }
        
    }
}

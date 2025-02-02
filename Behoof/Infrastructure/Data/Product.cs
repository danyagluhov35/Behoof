using System.Drawing;
using System.Text.Json.Serialization;
using Nancy.Json;

namespace Behoof.Infrastructure.Data
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid().ToString();
            DateCreate = DateTime.Now;
        }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageLink { get; set; }
        public decimal? Price { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? DateCreate { get; set; }

        public Category? Category { get; set; }
        public string? CategoryId { get; set; }

        public Camera? Camera { get; set; }
        public string? CameraId { get; set; }

        public System? System { get; set; }
        public string? SystemId { get; set; }

        public Power? Power { get; set; }
        public string? PowerId { get; set; }

        public Diagonal? Diagonal { get; set; }
        public string? DiagonalId { get; set; }

        public Network? Network { get; set; }
        public string? NetworkId { get; set; }

        public Memory? Memory { get; set; }
        public string? MemoryId { get; set; }

        public Color? Color { get; set; }
        public string? ColorId { get; set; }

        public YearOfRealise? YearOfRealise { get; set; }
        public string? YearOfRealiseId { get; set; }

        public int? BallDesign { get; set; }
        public int? BallBatery { get; set; }
        public int? BallCamera { get; set; }
        public int? BallAnswer { get; set; }
        public int? BallPortatable { get; set; }

        /*public List<Review> Review { get; set; } */// Отзывы
        public List<SupplierProduct>? SupplierProducts { get; set; }
        [JsonIgnore]
        public List<HistoryProduct>? HistoryProduct { get; set; }
    }

}

using Behoof.Infrastructure.Data;
using Behoof.Application.DTO;

namespace Behoof.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> Product { get; set; } = new List<Product>();
        public List<Category> Category { get; set; } = new List<Category>();
    }
}

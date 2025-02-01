using Behoof.Domain.Entity;

namespace Behoof.ViewModels
{
    public class FavoriteViewModel
    {
        public User User { get; set; } = new User();
        public List<SupplierItem> SupplierItems { get; set; } = new List<SupplierItem>();
        public List<Product> Products { get; set;} = new List<Product>();
    }
}

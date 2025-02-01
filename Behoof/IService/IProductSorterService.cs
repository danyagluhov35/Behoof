using Behoof.Domain.Entity;
using Behoof.Models.Product;

namespace Behoof.IService
{
    public interface IProductSorterService
    {
        Task<List<Product>> GetProductCateogry(string categoryId);
        Task<List<FoldProduct>> FoldProductValues(List<Product> product);
        Task<List<FoldProduct>> SortingValues(List<FoldProduct> product);
    }
}

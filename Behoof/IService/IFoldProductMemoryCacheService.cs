using Behoof.Models.Product;

namespace Behoof.IService
{
    public interface IFoldProductMemoryCacheService
    {
        Task<List<FoldProduct>> GetFoldProduct();
        Task SetFoldProduct(List<FoldProduct> FoldProduct);
    }
}

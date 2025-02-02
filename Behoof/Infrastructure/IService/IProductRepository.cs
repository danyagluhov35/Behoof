using Behoof.Application.DTO;
using Behoof.Infrastructure.Data;

namespace Behoof.Infrastructure.IService
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductCateogry(string categoryId);
        Task<List<FoldProductDto>> GetFoldProductValues(List<Product> product);
        Task<List<FoldProductDto>> SortingValues(List<FoldProductDto> product);
    }
}

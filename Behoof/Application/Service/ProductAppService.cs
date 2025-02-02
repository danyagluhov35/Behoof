using Behoof.Application.DTO;
using Behoof.Application.IService;
using Behoof.Infrastructure.Data;
using Behoof.Infrastructure.IService;

namespace Behoof.Application.Service
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductRepository ProductRepository;
        public ProductAppService(IProductRepository productService) => ProductRepository = productService;

        public async Task<List<FoldProductDto>> GetFoldProductValues(List<Product> product)
        {
            return await ProductRepository.GetFoldProductValues(product);
        }

        public async Task<List<Product>> GetProductCateogry(string categoryId)
        {
            return await ProductRepository.GetProductCateogry(categoryId);
        }

        public async Task<List<FoldProductDto>> SortingValues(List<FoldProductDto> product)
        {
            return await ProductRepository.SortingValues(product);
        }
    }
}

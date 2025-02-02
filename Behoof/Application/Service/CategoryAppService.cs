using Behoof.Application.IService;
using Behoof.Infrastructure.Data;
using Behoof.Infrastructure.IService;

namespace Behoof.Application.Service
{
    public class CategoryAppService : ICategoryAppService
    {
        private readonly ICategoryRepository CategoryRepository;
        public CategoryAppService(ICategoryRepository categoryService) => CategoryRepository = categoryService;
        public async Task<List<Category>> GetCategories()
        {
            return await CategoryRepository.GetCategories();
        }
    }
}

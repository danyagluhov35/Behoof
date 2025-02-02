using Behoof.Infrastructure.Data;
using Behoof.Infrastructure.IService;

namespace Behoof.Infrastructure.Service
{
    public class CategoryRepository : ICategoryRepository
    {
        private ApplicationContext db;
        public CategoryRepository(ApplicationContext db) => this.db = db;
        public async Task<List<Category>> GetCategories()
        {
            return db.Category.ToList();
        }
    }
}

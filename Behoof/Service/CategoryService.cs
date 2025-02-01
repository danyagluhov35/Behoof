using Behoof.Domain.Entity;
using Behoof.Domain.Entity.Context;
using Behoof.IService;

namespace Behoof.Service
{
    public class CategoryService : ICategoryService
    {
        private ApplicationContext db;
        public CategoryService(ApplicationContext db) => this.db = db;
        public async Task<List<Category>> GetCategories()
        {
            return db.Category.ToList();
        }
    }
}

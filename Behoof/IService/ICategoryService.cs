using Behoof.Domain.Entity;

namespace Behoof.IService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories();
    }
}

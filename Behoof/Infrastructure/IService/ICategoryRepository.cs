using Behoof.Infrastructure.Data;

namespace Behoof.Infrastructure.IService
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
    }
}

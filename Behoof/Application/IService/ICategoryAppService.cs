using Behoof.Infrastructure.Data;

namespace Behoof.Application.IService
{
    public interface ICategoryAppService
    {
        Task<List<Category>> GetCategories();
    }
}

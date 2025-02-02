using Behoof.Application.DTO;

namespace Behoof.Infrastructure.IService
{
    public interface IFoldProductMemoryCacheService
    {
        Task<List<FoldProductDto>> GetFoldProduct();
        Task SetFoldProduct(List<FoldProductDto> FoldProduct);
    }
}

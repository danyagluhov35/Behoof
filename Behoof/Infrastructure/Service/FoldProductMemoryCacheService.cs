using Behoof.Application.DTO;
using Behoof.Infrastructure.IService;
using Microsoft.Extensions.Caching.Memory;

namespace Behoof.Infrastructure.Service
{
    public class FoldProductMemoryCacheService : IFoldProductMemoryCacheService
    {
        private IMemoryCache Cache;
        public FoldProductMemoryCacheService(IMemoryCache cache)
        {
            Cache = cache;
        }

        public async Task SetFoldProduct(List<FoldProductDto> FoldProduct)
        {
            Cache.Set("FoldProduct", FoldProduct, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        }
        public async Task<List<FoldProductDto>> GetFoldProduct()
        {
            Cache.TryGetValue("FoldProduct", out List<FoldProductDto> FoldProduct);
            return FoldProduct!;
        }

    }
}

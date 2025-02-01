using Behoof.IService;
using Behoof.Models.Product;
using Microsoft.Extensions.Caching.Memory;

namespace Behoof.Service
{
    public class FoldProductMemoryCacheService : IFoldProductMemoryCacheService
    {
        private IMemoryCache Cache;
        public FoldProductMemoryCacheService(IMemoryCache cache)
        {
            Cache = cache;
        }

        public async Task SetFoldProduct(List<FoldProduct> FoldProduct)
        {
            Cache.Set("FoldProduct",FoldProduct, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
        }
        public async Task<List<FoldProduct>> GetFoldProduct()
        {
            Cache.TryGetValue("FoldProduct", out List<FoldProduct> FoldProduct);
            return FoldProduct!;
        }

    }
}

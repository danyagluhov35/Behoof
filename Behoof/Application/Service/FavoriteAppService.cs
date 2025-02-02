using Behoof.Application.IService;
using Behoof.Infrastructure.IService;

namespace Behoof.Application.Service
{
    public class FavoriteAppService : IFavoriteAppService
    {
        private readonly IFavoriteRepository FavoriteRepository;
        public FavoriteAppService(IFavoriteRepository favoriteService) => FavoriteRepository = favoriteService;
        public async Task Add(string productId, string userId)
        {
            await FavoriteRepository.Add(productId, userId);
        }
    }
}

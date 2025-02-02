using Microsoft.EntityFrameworkCore;
using Behoof.Infrastructure.Data;
using Behoof.Infrastructure.IService;

namespace Behoof.Infrastructure.Service
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private ApplicationContext db;
        public FavoriteRepository(ApplicationContext db)
        {
            this.db = db;
        }
        public async Task Add(string productId, string userId)
        {
            try
            {
                var user = db.Users.Include(u => u.Favorite).ThenInclude(f => f.FavoriteItem).FirstOrDefault(u => u.Id == userId);
                var product = db.Product.FirstOrDefault(p => p.Id == productId);


                if (product != null && user != null)
                    user.Favorite.FavoriteItem.Add(new FavoriteItem() { ProductId = product.Id });
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

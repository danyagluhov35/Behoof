using Behoof.Domain.Entity;
using Behoof.Domain.Entity.Context;
using Behoof.IService;
using Microsoft.EntityFrameworkCore;

namespace Behoof.Service
{
    public class FavoriteService : IFavoriteService
    {
        private ApplicationContext db;
        public FavoriteService(ApplicationContext db) 
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

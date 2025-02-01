using Behoof.Domain.Entity;
using Behoof.Domain.Entity.Context;
using Behoof.IService;
using Behoof.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace Behoof.Service
{
    public class ProductSorterService : IProductSorterService
    {
        private ApplicationContext db;
        public ProductSorterService(ApplicationContext db) => this.db = db;

        /// <summary>
        ///     Отбирается список товаров по категории
        /// </summary>
        /// <param name="categoryId">К какой категории относится товар/param>
        /// <returns></returns>
        public async Task<List<Product>> GetProductCateogry(string categoryId)
        {
            return db.Product.Include(p => p.Category).Where(p => p.Category.Id == categoryId).ToList();
        }
        /// <summary>
        ///     Возвращаем новый список, в котором имеется сумма значений всех параметров
        /// </summary>
        /// <param name="product">Принимает в себя список товаров</param>
        /// <returns></returns>
        public async Task<List<FoldProduct>> FoldProductValues(List<Product> product)
        {
            List<FoldProduct> foldProducts = new List<FoldProduct>();
            foreach(var item in product)
            {
                foldProducts.Add(new FoldProduct() 
                {
                    Id = item.Id,
                    Name = item.Name,
                    ImageLink = item.ImageLink,
                    CategoryName = item.Category.Name,
                    BallDesign = item.BallDesign,
                    BallBatery = item.BallBatery,
                    BallCamera = item.BallCamera,
                    BallAnswer = item.BallAnswer,
                    BallPortatable = item.BallPortatable,
                    SummBall = item.BallPortatable + item.BallDesign + item.BallBatery + item.BallCamera + item.BallAnswer
                });
            }
            return foldProducts;
        }


        public async Task<List<FoldProduct>> SortingValues(List<FoldProduct> product)
        {
            return product.OrderByDescending(p => p.SummBall).Take(4).ToList();
        }
    }
}

using Behoof.Application.DTO;
using Behoof.Infrastructure.Data;
using Behoof.Infrastructure.IService;
using Microsoft.EntityFrameworkCore;

namespace Behoof.Infrastructure.Service
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationContext db;
        public ProductRepository(ApplicationContext db) => this.db = db;

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
        public async Task<List<FoldProductDto>> GetFoldProductValues(List<Product> product)
        {
            List<FoldProductDto> foldProducts = new List<FoldProductDto>();
            foreach (var item in product)
            {
                foldProducts.Add(new FoldProductDto()
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


        public async Task<List<FoldProductDto>> SortingValues(List<FoldProductDto> product)
        {
            return product.OrderByDescending(p => p.SummBall).Take(4).ToList();
        }
    }
}

using Behoof.Domain.Entity.Context;
using Behoof.Domain.Enum;
using Behoof.IService;
using Behoof.Models.Product;
using Behoof.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Behoof.Controllers
{
    public class HomeController : Controller
    {
        private ICategoryService CategoryService;
        private IProductSorterService ProductSorterService;
        private IFoldProductMemoryCacheService FoldProductMemoryCacheService;
        private ApplicationContext db;
        public HomeController(ICategoryService categoryService, IProductSorterService productSorterService, IFoldProductMemoryCacheService foldProductMemoryCacheService, ApplicationContext db)
        {
            CategoryService = categoryService;
            ProductSorterService = productSorterService;
            FoldProductMemoryCacheService = foldProductMemoryCacheService;
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel() 
            {
                Product = db.Product.Where(p => p.DateCreate.Value.Day <= 30).Include(p => p.Category).Take(10).ToList(),
                Category = await CategoryService.GetCategories()
            };
            return View(model);
        }

        public async Task<IActionResult> GetProductToCategory([FromBody]string categoryId)
        {
            
            HttpContext.Session.SetString("CategoryId", categoryId);
            var listCategoryProduct = await ProductSorterService.GetProductCateogry(categoryId);
            var foldProduct = await ProductSorterService.FoldProductValues(listCategoryProduct);
            var sortFoldProduct = await ProductSorterService.SortingValues(foldProduct);

            await FoldProductMemoryCacheService.SetFoldProduct(sortFoldProduct);
           


            return ViewComponent("BestProduct", sortFoldProduct);
        }

        public async Task<IActionResult> GetProductToParameters(SortState sortState)
        {
            var categoryId = HttpContext.Session.GetString("CategoryId");
            List<FoldProduct> FoldProduct = await FoldProductMemoryCacheService.GetFoldProduct();
            if(FoldProduct == null)
            {
                var listCategoryProduct = await ProductSorterService.GetProductCateogry(categoryId);
                var foldProduct = await ProductSorterService.FoldProductValues(listCategoryProduct);
                var sortFoldProduct = await ProductSorterService.SortingValues(foldProduct);
                await FoldProductMemoryCacheService.SetFoldProduct(sortFoldProduct);
                FoldProduct = await FoldProductMemoryCacheService.GetFoldProduct();
            }

            if (sortState == SortState.DesignSort)
                FoldProduct = FoldProduct.OrderByDescending(p => p.BallDesign).ToList();
            else if(sortState == SortState.PortatableSort)
                FoldProduct = FoldProduct.OrderByDescending(p => p.BallPortatable).ToList();
            else if (sortState == SortState.CameraSort)
                FoldProduct = FoldProduct.OrderByDescending(p => p.BallCamera).ToList();

            return ViewComponent("BestProduct", FoldProduct);
        }
    }
}

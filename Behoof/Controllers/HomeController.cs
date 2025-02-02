using Behoof.Application.DTO;
using Behoof.Application.IService;
using Behoof.Core.Enums;
using Behoof.Infrastructure.Data;
using Behoof.Infrastructure.IService;
using Behoof.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Behoof.Controllers
{
    public class HomeController : Controller
    {
        private ICategoryAppService CategoryAppService;
        private IProductAppService ProductAppService;
        private IFoldProductMemoryCacheService FoldProductMemoryCacheService;
        private ApplicationContext db;
        public HomeController(ICategoryAppService categoryAppService, IProductAppService productAppService, IFoldProductMemoryCacheService foldProductMemoryCacheService, ApplicationContext db)
        {
            CategoryAppService = categoryAppService;
            ProductAppService = productAppService;
            FoldProductMemoryCacheService = foldProductMemoryCacheService;
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel() 
            {
                Product = db.Product.Where(p => p.DateCreate.Value.Day <= 30).Include(p => p.Category).Take(10).ToList(),
                Category = await CategoryAppService.GetCategories()
            };
            return View(model);
        }

        public async Task<IActionResult> GetProductToCategory([FromBody]string categoryId)
        {
            
            HttpContext.Session.SetString("CategoryId", categoryId);
            var listCategoryProduct = await ProductAppService.GetProductCateogry(categoryId);
            var foldProduct = await ProductAppService.GetFoldProductValues(listCategoryProduct);
            var sortFoldProduct = await ProductAppService.SortingValues(foldProduct);

            await FoldProductMemoryCacheService.SetFoldProduct(sortFoldProduct);
           


            return ViewComponent("BestProduct", sortFoldProduct);
        }

        public async Task<IActionResult> GetProductToParameters(SortState sortState)
        {
            var categoryId = HttpContext.Session.GetString("CategoryId");
            List<FoldProductDto> FoldProduct = await FoldProductMemoryCacheService.GetFoldProduct();
            if(FoldProduct == null)
            {
                var listCategoryProduct = await ProductAppService.GetProductCateogry(categoryId);
                var foldProduct = await ProductAppService.GetFoldProductValues(listCategoryProduct);
                var sortFoldProduct = await ProductAppService.SortingValues(foldProduct);
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

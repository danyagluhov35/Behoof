using System.Linq;
using Behoof.Domain.Entity;
using Behoof.Domain.Entity.Context;
using Behoof.IService;
using Behoof.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Behoof.Areas.Account.Controllers
{
    [Area("Account")]
    [Authorize]
    public class FavoriteController : Controller
    {
        private IFavoriteService FavoriteService;
        private IAccountService AccountService;
        private ApplicationContext db;
        public FavoriteController(IFavoriteService favoriteService, IAccountService accountService, ApplicationContext db)
        {
            FavoriteService = favoriteService;
            AccountService = accountService;
            this.db = db;
        }

        public IActionResult Favorite()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "Id")?.Value;

            var favoriteProduct = db.Users
                .Where(u => u.Id == userId)
                .Include(f => f.Favorite)
                .SelectMany(f => f.Favorite.FavoriteItem.Select(fi => fi.ProductId))
                .ToList();

            var supplierItems = db.SupplierItem
                .Where(s => s.Products.Any(p => favoriteProduct.Contains(p.Id)))
                .Select(g => new SupplierItem
                {
                    Id = g.Id,
                    MinPrice = g.MinPrice,
                    MaxPrice = g.MaxPrice,
                    Products = g.Products
                                .OrderBy(p => p.DateCreate)
                                .Select(p => new Product
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Price = p.Price,
                                    DateCreate = p.DateCreate,
                                    ImageLink = p.ImageLink,
                                })
                                .ToList()
                                        
                }).ToList();

            FavoriteViewModel model = new FavoriteViewModel()
            {
                SupplierItems = supplierItems
            };
            return View(model);
        }

        public async Task<IActionResult> Add(string productId)
        {
            var userId = await AccountService.IsRegistered();
            if (userId != null)
                await FavoriteService.Add(productId, userId);
            return new JsonResult(new { message = "404" });
        }
    }
}

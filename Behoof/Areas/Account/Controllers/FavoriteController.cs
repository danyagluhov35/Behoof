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


            var favoriteProduct = db.Favorite
                .Include(f => f.FavoriteItem)!
                .ThenInclude(fi => fi.Product)
                .ThenInclude(fi => fi.HistoryProduct)
                .Include(f => f.User)
                .Where(u => u.User.Id == userId)
                .SelectMany(f => f.FavoriteItem)
                .Select(g => new Product
                {
                    Id = g.Product.Id,
                    Name = g.Product.Name,
                    Price = g.Product.Price,
                    MaxPrice = g.Product.MaxPrice,
                    MinPrice = g.Product.MinPrice,
                    HistoryProduct = g.Product.HistoryProduct.Select(h => new HistoryProduct
                    {
                        DateUpdate = h.DateUpdate,
                    }).ToList()
                }).ToList()
                ;



            return View(favoriteProduct);
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

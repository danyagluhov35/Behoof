using Behoof.Domain.Entity;
using Behoof.Domain.Entity.Context;
using Behoof.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Behoof.Areas.Account.Controllers
{
    [Area("Account")]
    [Authorize]
    public class AccountController : Controller
    {
        private IAccountService AccountService;
        private ApplicationContext db;
        public AccountController(IAccountService accountService, ApplicationContext db)
        {
            AccountService = accountService;
            this.db= db;
        }
        public async Task<IActionResult> Profile()
        {
            return View
                (
                    db.Country.Include(c => c.City).ToList()
                );
        }

        public async Task<IActionResult> Exit()
        {
            HttpContext.Request.Headers.Remove("Authorization");
            HttpContext.Response.Cookies.Delete("JwtToken");

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public async Task<IActionResult> Delete(string id)
        {

            await AccountService.Delete(id);

            HttpContext.Request.Headers.Remove("Authorization");
            HttpContext.Response.Cookies.Delete("JwtToken");

            return RedirectToAction("Index", "Home", new {area = "" });
        }

        public async Task<IActionResult> Update(User? user)
        {
            var data = await AccountService.Update(user);
            await AccountService.Login(data);

            return new JsonResult(new { message = "" });
        }
    }
}

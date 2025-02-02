using Behoof.Application.DTO;
using Behoof.Application.IService;
using Behoof.Infrastructure.Data;
using Behoof.Infrastructure.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Behoof.Areas.Account.Controllers
{
    [Area("Account")]
    [Authorize]
    public class AccountController : Controller
    {
        private IAccountAppService AccountAppService;
        private ApplicationContext db;
        public AccountController(IAccountAppService accountAppService, ApplicationContext db)
        {
            AccountAppService = accountAppService;
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

            await AccountAppService.Delete(id);

            HttpContext.Request.Headers.Remove("Authorization");
            HttpContext.Response.Cookies.Delete("JwtToken");

            return RedirectToAction("Index", "Home", new {area = "" });
        }

        public async Task<IActionResult> Update(UserUpdateDto? userUpdateDto)
        {
            var data = await AccountAppService.Update(userUpdateDto);
            //await AccountService.Login(data);

            return new JsonResult(new { message = "" });
        }
    }
}

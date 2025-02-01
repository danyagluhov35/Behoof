using Behoof.Domain.Entity;
using Behoof.IService;
using Behoof.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Behoof.Controllers
{
    public class AuthorizationController : Controller
    {
        private IAccountService AccountService;
        public AuthorizationController(IAccountService accountService)
        {
            AccountService = accountService;
        }
        public async Task<IActionResult> Authorization()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Authorization(UserLogin user)
        {
            if (ModelState.IsValid)
            {
                var data = await AccountService.Validation(user);
                if (data != null)
                {
                    await AccountService.Login(data);
                    return RedirectToAction("Profile", "Account", new { area = "Account" });
                }
            }
            ModelState.AddModelError("", "Пользователь не найден");
            return View();
        }
    }
}

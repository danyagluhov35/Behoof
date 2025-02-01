using Behoof.Domain.Entity;
using Behoof.IService;
using Behoof.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Behoof.Controllers
{
    public class RegistrationController : Controller
    {
        private IAccountService AccountService;
        public RegistrationController(IAccountService accountService)
        {
            AccountService = accountService;
        }
        public async Task<IActionResult> Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(UserRegister user)
        {
            if (ModelState.IsValid)
            {
                await AccountService.Register(user);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}

using Behoof.Application.DTO;
using Behoof.Application.IService;
using Behoof.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Behoof.Controllers
{
    public class AuthorizationController : Controller
    {
        private IAccountAppService AccountAppService;
        public AuthorizationController(IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }
        public async Task<IActionResult> Authorization()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Authorization(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await AccountAppService.Login(userLoginDto);
                if(result != null)
                    return RedirectToAction("Profile", "Account", new { area = "Account" });
                ModelState.AddModelError("", "Пользователь не найден");
            }
            return View();
        }
    }
}

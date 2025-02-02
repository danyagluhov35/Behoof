using Microsoft.AspNetCore.Mvc;
using Behoof.Application.IService;
using Behoof.Application.DTO;
using Behoof.Application.Service;

namespace Behoof.Controllers
{
    public class RegistrationController : Controller
    {
        private IAccountAppService AccountAppService;
        public RegistrationController(IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }
        public async Task<IActionResult> Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(UserRegisterDto userRegisterDto)
        {
            if (ModelState.IsValid)
            {
                await AccountAppService.Register(userRegisterDto);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}

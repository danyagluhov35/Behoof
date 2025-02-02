using Behoof.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Behoof.ViewComponents
{
    public class BestProductViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<FoldProductDto> product)
        {
            return View(product);
        }
    }
}

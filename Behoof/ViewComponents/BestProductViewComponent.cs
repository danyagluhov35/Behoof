using Behoof.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace Behoof.ViewComponents
{
    public class BestProductViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<FoldProduct> product)
        {
            return View(product);
        }
    }
}

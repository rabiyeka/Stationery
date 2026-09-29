using Microsoft.AspNetCore.Mvc;

namespace Stationery.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBaseController : Controller
    {
        protected IActionResult? RequireAdmin()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login","Account", new { area="", returnUrl=Request.Path});
            }
            if (!User.IsInRole("Admin"))
            {
                TempData["Error"]="Bu sayfaya erişim yetkiniz yok.";
                return RedirectToAction("Index","Home",new {area=""});
            }
            return null;
        }
    }
}

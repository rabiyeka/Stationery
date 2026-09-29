using Microsoft.AspNetCore.Mvc;

namespace Stationery.Areas.Admin.Controllers
{
    
    public class HomeController : AdminBaseController
    {
        // GET: HomeController
        public IActionResult Index()
        {
            var redirect = RequireAdmin();
            return redirect ?? View();
        }

    }
}

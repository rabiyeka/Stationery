using Microsoft.AspNetCore.Mvc;

namespace Stationery.Areas.Admin.Controllers
{
    
    public class HomeController : AdminBaseController
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

    }
}

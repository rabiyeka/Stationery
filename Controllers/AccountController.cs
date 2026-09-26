using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stationery.Models;

namespace Stationery.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
        private readonly UserManager <StationeryUser> _userManager;
        private readonly SignInManager <StationeryUser> _signInManager;

        public AccountController(UserManager<StationeryUser> userManager, SignInManager<StationeryUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}

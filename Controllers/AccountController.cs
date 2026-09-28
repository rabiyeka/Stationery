using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stationery.Data;
using Stationery.Models;
using Stationery.ViewModels.Account;
using Stationery.Services;
using Stationery.ViewModels.Orders;

namespace Stationery.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
        private readonly UserManager<StationeryUser> _userManager;
        private readonly SignInManager<StationeryUser> _signInManager;
        private readonly StationeryDbContext _context;
        private readonly IOrderService _orderService;

        public AccountController(UserManager<StationeryUser> userManager, SignInManager<StationeryUser> signInManager, StationeryDbContext context, IOrderService orderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            return View(new AccountIndexViewModel
            {
                FullName = user.FullName ?? string.Empty,
                Email = user.Email ?? string.Empty
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Challenge();
            return View(await _orderService.GetOrdersForUserAsync(userId));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Addresses()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Challenge();
            return View(new AddressesViewModel { Items = await GetAddressesAsync(userId) });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Addresses(AddressesViewModel pageModel)
        {
            var model = pageModel.Form;
            if (!ModelState.IsValid)
            {
                var invalidUserId = _userManager.GetUserId(User);
                if (invalidUserId is not null)
                {
                    pageModel.Items = await GetAddressesAsync(invalidUserId);
                }

                return View(pageModel);
            }

            var userId = _userManager.GetUserId(User);
            if (userId is null) return Challenge();

            _context.Addresses.Add(new Address
            {
                UserId = userId,
                Title = model.Title,
                FullName = model.FullName,
                Phone = model.Phone,
                City = model.City,
                District = model.District,
                Neighborhood = model.Neighborhood,
                Detail = model.Detail,
                PostalCode = model.PostalCode
            });
            await _context.SaveChangesAsync();
            TempData["Success"] = "Adresiniz kaydedildi.";
            return RedirectToAction(nameof(Addresses));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userId = _userManager.GetUserId(User);
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
            if (address is not null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Addresses));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Membership()
        {
            return View(new ChangePasswordViewModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Membership(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user is null) return Challenge();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Success"] = "Şifreniz güncellendi.";
                return RedirectToAction(nameof(Membership));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        [HttpGet]
        public ActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            if (user is not null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task<IList<AddressViewModel>> GetAddressesAsync(string userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.Title)
                .Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    FullName = a.FullName,
                    Phone = a.Phone,
                    City = a.City,
                    District = a.District,
                    Neighborhood = a.Neighborhood,
                    Detail = a.Detail,
                    PostalCode = a.PostalCode
                })
                .ToListAsync();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new StationeryUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, TranslateIdentityError(error.Code));
            }

            return View(model);
        }

        private static string TranslateIdentityError(string errorCode)
        {
            return errorCode switch
            {
                "DuplicateUserName" or "DuplicateEmail" => "Bu e-posta adresi zaten kayıtlı.",
                "PasswordTooShort" => "Şifre en az 8 karakter olmalıdır.",
                "PasswordRequiresUniqueChars" => "Şifre en az bir farklı karakter içermelidir.",
                "PasswordRequiresNonAlphanumeric" => "Şifre en az bir özel karakter içermelidir.",
                "PasswordRequiresDigit" => "Şifre en az bir rakam içermelidir.",
                "PasswordRequiresLower" => "Şifre en az bir küçük harf içermelidir.",
                "PasswordRequiresUpper" => "Şifre en az bir büyük harf içermelidir.",
                _ => "Kayıt oluşturulurken bir hata oluştu."
            };
        }

    }
}

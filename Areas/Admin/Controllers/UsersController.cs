using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Stationery.Services;
using Stationery.ViewModels.Admin;

namespace Stationery.Areas.Admin.Controllers
{
    public class UsersController : AdminBaseController
    {
        // GET: UsersController
        private readonly IUserAdminService _userAdminService;

        public UsersController(IUserAdminService userAdminService)
        {
            _userAdminService = userAdminService;
        }

        public async Task<IActionResult> Index()
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            var users = await _userAdminService.GetUsersForAdminAsync();
            return View(users);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var redirect = RequireAdmin();
            return redirect ?? View(new AdminUserCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminUserCreateViewModel model)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var (success, error) = await _userAdminService.CreateUserAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "Kullanıcı oluşuturulamadı.");
                return View(model);
            }
            TempData["Success"] = "Kullanıcı başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            var model = await _userAdminService.GetUserForEditAsync(id);
            return model is null ? NotFound() : View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AdminUserFormViewModel model)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            if (!ModelState.IsValid)
            {
                model.Id = id;
                return View(model);
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var (success, error) = await _userAdminService.UpdateUserAsync(id, model, currentUserId);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "Kullanıcı güncellenemedi.");
                model.Id = id;
                return View(model);
            }
            TempData["Success"] = "Kullanıcı bilgileri başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLock(string id)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            var model = await _userAdminService.GetUserForEditAsync(id);
            if (model is null) return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var (success, error) = await _userAdminService.SetLockoutAsync(id, !model.IsLocked, currentUserId);

            TempData[success ? "Success" : "Error"] = success ? (model.IsLocked ? "Hesap açıldı" : "Hesap kilitlendi.") : error;
            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var (success, error) = await _userAdminService.DeleteUserAsync(id, currentUserId);
            TempData[success ? "Success" : "Error"] = success ? "Kullanıcı silindi, spet ve sipariş bilgileri de kaldırıldı." : error;
            return RedirectToAction(nameof(Index));
        }

    }
}

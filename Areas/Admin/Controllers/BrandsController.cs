using Microsoft.AspNetCore.Mvc;
using Stationery.Services;
using Stationery.ViewModels.Admin;

namespace Stationery.Areas.Admin.Controllers
{
    public class BrandsController : AdminBaseController
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;

            var model = new AdminBrandIndexViewModel
            {
                Brands = await _brandService.GetBrandsForAdminAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminBrandIndexViewModel model)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;

            if (!ModelState.IsValid)
            {
                model.Brands = await _brandService.GetBrandsForAdminAsync();
                return View("Index", model);
            }

            var (success, error) = await _brandService.CreateBrandAsync(model.Form.Name);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "Marka eklenemedi.");
                model.Brands = await _brandService.GetBrandsForAdminAsync();
                return View("Index", model);
            }

            TempData["Success"] = "Marka eklendi.";
            return RedirectToAction(nameof(Index));
        }
    }
}

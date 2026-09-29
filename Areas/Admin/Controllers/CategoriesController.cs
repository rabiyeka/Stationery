using Microsoft.AspNetCore.Mvc;
using Stationery.Services;
using Stationery.ViewModels.Admin;

namespace Stationery.Areas.Admin.Controllers
{
    public class CategoriesController : AdminBaseController
    {
        // GET: CategoriesController
       private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public  async Task<IActionResult> Index()
        {
            var redirect = RequireAdmin();
            if(redirect is not null) return redirect;
            var categories = await _categoryService.GetCategoriesForAdminAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var redirect = RequireAdmin();
            return redirect ?? View(new AdminCategoryFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCategoryFormViewModel model)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var (success, error) = await _categoryService.CreateCategoryAsync(model.Name, model.Description);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "Kategori oluşturulamadı.");
                return View(model);
            }
            TempData["Success"]="Kategori eklendi";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            var model = await _categoryService.GetCategoryForEditAsync(id);
            return model is null ? NotFound() : View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminCategoryFormViewModel model)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            if (!ModelState.IsValid)
            {
                model.Id=id;
                return View(model);
            }
            var (success, error) = await _categoryService.UpdateCategoryAsync(id, model.Name, model.Description);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "Kategori güncellenemedi.");
                model.Id=id;
                return View(model);
            }
            TempData["Success"] = "Kategori güncellendi";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            var (success, error) = await _categoryService.DeleteCategoryAsync(id);
            TempData[success ? "Success" : "Error"]=success? "Kategori silindi." : error;
            return RedirectToAction(nameof(Index));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Stationery.Services;
using Stationery.ViewModels.Admin;

namespace Stationery.Areas.Admin.Controllers
{
    public class ProductsController : AdminBaseController
    {
        // GET: ProductsController
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IWebHostEnvironment _env;

        public ProductsController(
            IProductService productService,
            ICategoryService categoryService,
            IBrandService brandService,
            IWebHostEnvironment env)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var redirect = RequireAdmin();
            if (redirect is not null)
            {
                return redirect;
            }

            var products = await _productService.GetProductsForAdminAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var redirect = RequireAdmin();
            if (redirect is not null)
            {
                return redirect;
            }

            await LoadCategoriesAsync();
            return View(new AdminProductFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminProductFormViewModel model)
        {
            var redirect = RequireAdmin();
            if (redirect is not null)
            {
                return redirect;
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(model);
            }

            var (success, productId, error) = await _productService.CreateProductAsync(model);
            if (!success || productId is null)
            {
                ModelState.AddModelError(string.Empty, error ?? "Ürün oluşturulamadı.");
                await LoadCategoriesAsync();
                return View(model);
            }

            TempData["Success"] = "Ürün eklendi.";
            return RedirectToAction(nameof(Edit), new { id = productId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var redirect = RequireAdmin();
            if (redirect is not null)
            {
                return redirect;
            }

            var model = await _productService.GetProductForEditAsync(id);
            if (model is null)
            {
                return NotFound();
            }

            await LoadCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminProductFormViewModel model)
        {
            var redirect = RequireAdmin();
            if (redirect is not null)
            {
                return redirect;
            }

            model.Id = id;
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(model);
            }

            var (success, error) = await _productService.UpdateProductAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "Ürün güncellenemedi.");
                await LoadCategoriesAsync();
                return View(model);
            }

            TempData["Success"] = "Ürün güncellendi.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var redirect = RequireAdmin();
            if (redirect is not null)
            {
                return redirect;
            }

            var (success, error) = await _productService.DeleteProductAsync(id);
            TempData[success ? "Success" : "Error"] = success ? "Ürün silindi." : error;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(int id, IFormFile? imageFile)
        {
            var redirect = RequireAdmin();
            if (redirect is not null)
            {
                return redirect;
            }

            if (imageFile is null || imageFile.Length == 0)
            {
                TempData["Error"] = "Lütfen bir dosya seçin.";
                return RedirectToAction(nameof(Edit), new { id });
            }

            var (success, _, error) = await _productService.UploadProductImageAsync(id, imageFile, _env);
            TempData[success ? "Success" : "Error"] = success ? "Görsel yüklendi." : error;
            return RedirectToAction(nameof(Edit), new { id });
        }

        private async Task LoadCategoriesAsync()
        {
            ViewBag.Categories = await _categoryService.GetCategoriesForAdminAsync();
            ViewBag.Brands = await _brandService.GetBrandsForAdminAsync();
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stationery.Services;
using Stationery.ViewModels.Products;

namespace Stationery.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICartService _cartService;
        private readonly IBrandService _brandService;

        public ProductsController(IProductService productService, ICategoryService categoryService, ICartService cartService, IBrandService brandService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _cartService = cartService;
            _brandService = brandService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var model = new ProductsIndexViewModel
            {
                Products = await _productService.GetAllProductsAsync(categoryId),
                Categories = await _categoryService.GetCategoriesForFilterAsync(),
                SelectedCategoryId = categoryId,
                Brands = await _brandService.GetAllBrandsForFilterAsync()
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            if(id <= 0) return NotFound();
            var product = await _productService.GetProductDetailsAsync(id);
            return product is null? NotFound() : View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1, string? returnUrl = null)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if(string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }
            var (success, error) = await _cartService.AddItemAsync(userId, productId, quantity);
            if (!success)
            {
                TempData["Error"] = error;
                return Redirect(returnUrl ?? Url.Action("Details",new {id=productId})!);
            }
            TempData["Success"]="Ürün sepete eklendi.";
            return Redirect(returnUrl ?? Url.Action("Index","Cart")!);
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stationery.Data; // DbContext'inizin olduğu namespace
using Stationery.Extensions;
using Stationery.Services;
using Stationery.ViewModels;

namespace Stationery.Controllers
{
    public class CartsController : Controller
    {
        private readonly ICartService _cartService;
        private readonly StationeryDbContext _context; 

        public CartsController(ICartService cartService, StationeryDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        // 1. SEPETİM SAYFASI (Üye olan DB'den, Misafir Session'dan görür)
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var userId = GetUserId();

            // Kullanıcı giriş YAPMIŞSA veritabanındaki sepeti getir
            if (userId != null)
            {
                var model = await _cartService.GetCartAsync(userId);
                return View(model);
            }

            // Kullanıcı GİRİŞ YAPMAMIŞSA Session'daki sepeti getir
            var sessionCart = HttpContext.Session.GetObjectFromJson<List<CartSessionItem>>("Cart") ?? new List<CartSessionItem>();
            return View("SessionIndex", sessionCart); // Veya doğrudan sessionCart modelini dönen View
        }

        // 2. SEPETE EKLE (Giriş zorunlu DEĞİL)
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userId = GetUserId();

            // Kullanıcı giriş yapmışsa mevcut servisinizle ekleyin (Eğer servisiniz destekliyorsa)
            // Ya da herkes için hızlıca Session'a ekleyelim:
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartSessionItem>>("Cart") ?? new List<CartSessionItem>();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartSessionItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            TempData["Success"] = "Ürün sepete eklendi.";

            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int cartItemId, int quantity)
        {
            var userId = GetUserId();
            if (userId is null) return NotFound();
            (bool success, string? error) = await _cartService.UpdateItemQuantityAsync(userId, cartItemId, quantity);
            TempData[success ? "Success" : "Error"] = success ? "Sepet güncellendi." : error;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            var userId = GetUserId();
            if (userId is null) return NotFound();
            var (success, error) = await _cartService.RemoveItemAsync(userId, cartItemId);
            TempData[success ? "Success" : "Error"] = success ? "Ürün sepetten çıkarıldı." : error;
            return RedirectToAction(nameof(Index));
        }

        // 3. SATIN AL / İŞLEMİ TAMAMLA (Sadece Giriş Yapanlar)
        [Authorize] // Giriş yapılmamışsa ASP.NET Core otomatik Giriş/Kayıt sayfasına yönlendirir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();
            if (userId is null) return Unauthorized();

            // Sipariş oluşturma işlemleri...
            return RedirectToAction(nameof(Index));
        }

        private string? GetUserId() => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    }
}
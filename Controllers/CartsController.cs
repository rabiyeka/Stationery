using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stationery.Data; // DbContext'inizin olduğu namespace
using Stationery.Extensions;
using Stationery.Services;
using Stationery.ViewModels;
using Stationery.ViewModels.Account;
using Stationery.ViewModels.Orders;

namespace Stationery.Controllers
{
    public class CartsController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly StationeryDbContext _context; 

        public CartsController(ICartService cartService, IOrderService orderService, StationeryDbContext context)
        {
            _cartService = cartService;
            _orderService = orderService;
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
                await MergeSessionCartAsync(userId);
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
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            if (quantity < 1)
            {
                quantity = 1;
            }

            var userId = GetUserId();

            if (userId is not null)
            {
                var result = await _cartService.AddItemAsync(userId, productId, quantity);
                TempData[result.Success ? "Success" : "Error"] = result.Success ? "Ürün sepete eklendi." : result.ErrorMessage;
                return RedirectToAction("Index", "Products");
            }

            // Kullanıcı giriş yapmışsa mevcut servisinizle ekleyin (Eğer servisiniz destekliyorsa)
            // Ya da herkes için hızlıca Session'a ekleyelim:
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartSessionItem>>("Cart") ?? new List<CartSessionItem>();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartSessionItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            TempData["Success"] = "Ürün sepete eklendi.";

            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int cartItemId, int quantity, int productId = 0)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                var sessionCart = HttpContext.Session.GetObjectFromJson<List<CartSessionItem>>("Cart") ?? [];
                var sessionItem = sessionCart.FirstOrDefault(item => item.ProductId == productId);

                if (sessionItem is null || quantity < 1)
                {
                    return NotFound();
                }

                sessionItem.Quantity = quantity;
                HttpContext.Session.SetObjectAsJson("Cart", sessionCart);
                TempData["Success"] = "Sepet güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            (bool success, string? error) = await _cartService.UpdateItemQuantityAsync(userId, cartItemId, quantity);
            TempData[success ? "Success" : "Error"] = success ? "Sepet güncellendi." : error;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int cartItemId, int productId = 0)
        {
            var userId = GetUserId();
            if (userId is null)
            {
                var sessionCart = HttpContext.Session.GetObjectFromJson<List<CartSessionItem>>("Cart") ?? [];
                var removed = sessionCart.RemoveAll(item => item.ProductId == productId);

                if (removed == 0)
                {
                    return NotFound();
                }

                HttpContext.Session.SetObjectAsJson("Cart", sessionCart);
                TempData["Success"] = "Ürün sepetten çıkarıldı.";
                return RedirectToAction(nameof(Index));
            }

            var (success, error) = await _cartService.RemoveItemAsync(userId, cartItemId);
            TempData[success ? "Success" : "Error"] = success ? "Ürün sepetten çıkarıldı." : error;
            return RedirectToAction(nameof(Index));
        }

        // 3. SATIN AL / İŞLEMİ TAMAMLA (Sadece Giriş Yapanlar)
        [Authorize]
        [HttpGet]
        [ActionName(nameof(Checkout))]
        public async Task<IActionResult> CheckoutPage()
        {
            var userId = GetUserId();
            if (userId is null) return Challenge();

            return View("Checkout", await BuildCheckoutModelAsync(userId));
        }

        [Authorize] // Giriş yapılmamışsa ASP.NET Core otomatik Giriş/Kayıt sayfasına yönlendirir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var userId = GetUserId();
            if (userId is null) return Unauthorized();

            var address = model.AddressId.HasValue
                ? await _context.Addresses.FirstOrDefaultAsync(a => a.Id == model.AddressId && a.UserId == userId)
                : null;

            if (address is null)
            {
                ModelState.AddModelError(nameof(model.AddressId), "Lütfen geçerli bir teslimat adresi seçiniz.");
            }

            if (!ModelState.IsValid)
            {
                model.Addresses = await GetAddressViewModelsAsync(userId);
                model.Cart = await _cartService.GetCartAsync(userId);
                return View("Checkout", model);
            }

            var shippingAddress = string.Join(", ", new[]
            {
                address!.FullName,
                address.Detail,
                address.Neighborhood,
                $"{address.District} / {address.City}",
                address.PostalCode
            }.Where(value => !string.IsNullOrWhiteSpace(value)));

            var result = await _orderService.CreateOrderAsync(userId, shippingAddress, model.PaymentMethod);
            if (!result.Success)
            {
                TempData["Error"] = result.Error ?? "Sipariş oluşturulamadı.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Siparişiniz başarıyla oluşturuldu.";
            return RedirectToAction("Orders", "Account");
        }

        private string? GetUserId() => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        private async Task MergeSessionCartAsync(string userId)
        {
            var sessionCart = HttpContext.Session.GetObjectFromJson<List<CartSessionItem>>("Cart");
            if (sessionCart is null || sessionCart.Count == 0)
            {
                return;
            }

            foreach (var item in sessionCart)
            {
                await _cartService.AddItemAsync(userId, item.ProductId, item.Quantity);
            }

            HttpContext.Session.Remove("Cart");
        }

        private async Task<CheckoutViewModel> BuildCheckoutModelAsync(string userId)
        {
            return new CheckoutViewModel
            {
                Addresses = await GetAddressViewModelsAsync(userId),
                Cart = await _cartService.GetCartAsync(userId)
            };
        }

        private async Task<IList<AddressViewModel>> GetAddressViewModelsAsync(string userId)
        {
            return await _context.Addresses
                .Where(address => address.UserId == userId)
                .OrderBy(address => address.Title)
                .Select(address => new AddressViewModel
                {
                    Id = address.Id,
                    Title = address.Title,
                    FullName = address.FullName,
                    Phone = address.Phone,
                    City = address.City,
                    District = address.District,
                    Neighborhood = address.Neighborhood,
                    Detail = address.Detail,
                    PostalCode = address.PostalCode
                })
                .ToListAsync();
        }
    }
}
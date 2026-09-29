using Microsoft.AspNetCore.Mvc;
using Stationery.Models.Enums;
using Stationery.Services;

namespace Stationery.Areas.Admin.Controllers
{
    public class OrdersController : AdminBaseController
    {
        // GET: OrdersController
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            var orders = await _orderService.GetAllOrdersForAdminAsync();
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var redirect = RequireAdmin();
            if (redirect is not null) return redirect;
            var (success, error) = await _orderService.UpdateOrderStatusAsync(id, status);
            TempData[success ? "Success" : "Error"] = success
                ? $"Sipariş #{id} durumu {status} olarak güncellendi."
                : error;
            return RedirectToAction(nameof(Index));
        }


    }
}

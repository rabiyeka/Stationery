using System.ComponentModel.DataAnnotations;
using Stationery.ViewModels.Account;
using Stationery.ViewModels.Carts;

namespace Stationery.ViewModels.Orders;

public class CheckoutViewModel
{
    [Required(ErrorMessage = "Lütfen bir teslimat adresi seçiniz.")]
    public int? AddressId { get; set; }

    [Required(ErrorMessage = "Lütfen bir ödeme yöntemi seçiniz.")]
    public string PaymentMethod { get; set; } = string.Empty;

    public IList<AddressViewModel> Addresses { get; set; } = [];
    public CartIndexViewModel Cart { get; set; } = new();
}
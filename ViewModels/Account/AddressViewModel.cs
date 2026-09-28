using System.ComponentModel.DataAnnotations;

namespace Stationery.ViewModels.Account;

public class AddressViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Adres başlığı zorunludur.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ad soyad zorunludur.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon zorunludur.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "İl zorunludur.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "İlçe zorunludur.")]
    public string District { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mahalle zorunludur.")]
    public string Neighborhood { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres detayı zorunludur.")]
    public string Detail { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;
}
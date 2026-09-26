using System;
using System.ComponentModel.DataAnnotations;
namespace Stationery.ViewModels.Admin;

public class AdminUserCreateViewModel
{
    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin.")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = null!;

    [MaxLength(100)]
    [Display(Name = "Ad Soyad")]
    public string? FullName { get; set; }

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
    [DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Şifre (Tekrar)")]
    [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor.")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "Rol seçin.")]
    [Display(Name = "Rol")]
    public string Role { get; set; } = "Customer";
}



using System;
using System.ComponentModel.DataAnnotations;
namespace Stationery.ViewModels.Admin;

public class AdminUserFormViewModel
{
    public string Id { get; set; } = null!;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin.")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = null!;

    [MaxLength(100)]
    [Display(Name = "Ad Soyad")]
    public string? FullName { get; set; }

    [Required(ErrorMessage = "Rol seçin.")]
    [Display(Name = "Rol")]
    public string Role { get; set; } = "Customer";

    public bool IsLocked { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre")]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre (Tekrar)")]
    [Compare(nameof(NewPassword), ErrorMessage = "Şifreler eşleşmiyor.")]
    public string? ConfirmNewPassword { get; set; }

}

using System.ComponentModel.DataAnnotations;

namespace Stationery.ViewModels.Account;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Mevcut şifre zorunludur.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mevcut şifre")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yeni şifre zorunludur.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Yeni şifre en az 8 karakter olmalıdır.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "Şifre en az 8 karakter olmalı ve en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.")]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni şifre")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yeni şifre tekrarı zorunludur.")]
    [Compare(nameof(NewPassword), ErrorMessage = "Şifreler eşleşmiyor.")]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni şifre tekrarı")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Stationery.ViewModels.Admin;

public class AdminBrandFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Marka adı zorunludur.")]
    [MaxLength(50, ErrorMessage = "Marka adı en fazla 50 karakter olabilir.")]
    [Display(Name = "Marka Adı")]
    public string Name { get; set; } = null!;

}

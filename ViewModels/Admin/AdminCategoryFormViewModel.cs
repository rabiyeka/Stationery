using System;
using System.ComponentModel.DataAnnotations;

namespace Stationery.ViewModels.Admin;

public class AdminCategoryFormViewModel
{
    public int? Id { get; set; }
    
    [Required(ErrorMessage = "Kategori adı zorunludur.")]
    [Display(Name = "Kategori Adı")]
    public string Name { get; set; }= null!;

    [Display(Name = "Açıklama")]
    [MaxLength(100, ErrorMessage = "Açıklama en fazla 100 karakter olabilir.")]
    public string? Description { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Stationery.ViewModels.Admin;

public class AdminProductFormViewModel
{
    public int? Id { get; set; }


    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [Display(Name = "Ürün Adı")]
    public string Name { get; set; } = null!;


    [MaxLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
    [Display(Name = "Ürün Açıklaması")]
    public string Description { get; set; } = null!;


    [Range(0.01, double.MaxValue)]
    [Display(Name = "Fiyat")]
    public decimal Price { get; set; }


    [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya daha büyük olmalıdır.")]
    [Display(Name = "Stok Miktarı")]
    public int StockQuantity { get; set; }


    [Required(ErrorMessage = "Görsel seçimi zorunludur.")]
    [MaxLength(500)]
    [Display(Name = "Resim URL")]
    public string ImageUrl { get; set; } = "https://placehold.co/400x400?text=Yeni+Ürün";


    [Range(1, int.MaxValue)]
    [Display(Name = "Kategori")]
    public int CategoryId { get; set; }


    [Range(1, int.MaxValue)]
    [Display(Name = "Marka")]
    public int BrandId { get; set; }
}

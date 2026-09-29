namespace Stationery.ViewModels.Admin;

public class AdminBrandIndexViewModel
{
    public IList<AdminBrandListViewModel> Brands { get; set; } = [];
    public AdminBrandFormViewModel Form { get; set; } = new();
}
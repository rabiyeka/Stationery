namespace Stationery.ViewModels.Account;

public class AddressesViewModel
{
    public AddressViewModel Form { get; set; } = new();
    public IList<AddressViewModel> Items { get; set; } = [];
}

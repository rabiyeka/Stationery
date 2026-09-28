namespace Stationery.Models;

public class Address
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public StationeryUser? User { get; set; }
    public string Title { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}
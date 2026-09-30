namespace Stationery.Services;

public interface ILocationService
{
    Task<IReadOnlyList<LocationOption>> GetProvincesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LocationOption>> GetDistrictsAsync(int provinceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LocationOption>> GetNeighborhoodsAsync(int districtId, CancellationToken cancellationToken = default);
}
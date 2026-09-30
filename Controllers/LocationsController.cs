using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stationery.Services;

namespace Stationery.Controllers;

[Authorize]
[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet("provinces")]
    public async Task<ActionResult<IReadOnlyList<LocationOption>>> GetProvinces(CancellationToken cancellationToken)
    {
        return await GetLocationsAsync(() => _locationService.GetProvincesAsync(cancellationToken));
    }

    [HttpGet("provinces/{provinceId:int}/districts")]
    public async Task<ActionResult<IReadOnlyList<LocationOption>>> GetDistricts(int provinceId, CancellationToken cancellationToken)
    {
        if (provinceId <= 0) return BadRequest(new { message = "Geçersiz il seçimi." });
        return await GetLocationsAsync(() => _locationService.GetDistrictsAsync(provinceId, cancellationToken));
    }

    [HttpGet("districts/{districtId:int}/neighborhoods")]
    public async Task<ActionResult<IReadOnlyList<LocationOption>>> GetNeighborhoods(int districtId, CancellationToken cancellationToken)
    {
        if (districtId <= 0) return BadRequest(new { message = "Geçersiz ilçe seçimi." });
        return await GetLocationsAsync(() => _locationService.GetNeighborhoodsAsync(districtId, cancellationToken));
    }

    private async Task<ActionResult<IReadOnlyList<LocationOption>>> GetLocationsAsync(
        Func<Task<IReadOnlyList<LocationOption>>> getLocations)
    {
        try
        {
            return Ok(await getLocations());
        }
        catch (HttpRequestException)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = "Konum listesine şu anda ulaşılamıyor." });
        }
        catch (TaskCanceledException)
        {
            return StatusCode(StatusCodes.Status504GatewayTimeout, new { message = "Konum listesi isteği zaman aşımına uğradı." });
        }
    }
}
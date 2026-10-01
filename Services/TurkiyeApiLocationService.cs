using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;

namespace Stationery.Services;

public sealed class TurkiyeApiLocationService : ILocationService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public TurkiyeApiLocationService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public Task<IReadOnlyList<LocationOption>> GetProvincesAsync(CancellationToken cancellationToken = default)
    {
        return GetCachedLocationsAsync(
            "locations:provinces",
            "v2/provinces?fields=id,name&limit=100",
            cancellationToken);
    }

    public Task<IReadOnlyList<LocationOption>> GetDistrictsAsync(int provinceId, CancellationToken cancellationToken = default)
    {
        return GetCachedLocationsAsync(
            $"locations:provinces:{provinceId}:districts",
            $"v2/provinces/{provinceId}/districts?fields=id,name&limit=1000",
            cancellationToken);
    }

    public Task<IReadOnlyList<LocationOption>> GetNeighborhoodsAsync(int districtId, CancellationToken cancellationToken = default)
    {
        return GetCachedLocationsAsync(
            $"locations:districts:{districtId}:neighborhoods",
            $"v2/districts/{districtId}/neighborhoods?fields=id,name,postalCode&limit=1000",
            cancellationToken);
    }

    private async Task<IReadOnlyList<LocationOption>> GetCachedLocationsAsync(
        string cacheKey,
        string requestUri,
        CancellationToken cancellationToken)
    {
        var locations = await _cache.GetOrCreateAsync<IReadOnlyList<LocationOption>>(
            cacheKey,
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;

                using var response = await _httpClient.GetAsync(requestUri, cancellationToken);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<LocationApiResponse>(cancellationToken: cancellationToken);
                return result?.Data ?? [];
            });

        return locations ?? Array.Empty<LocationOption>();
    }

    private sealed class LocationApiResponse
    {
        [JsonPropertyName("data")]
        public List<LocationOption>? Data { get; set; }
    }
}
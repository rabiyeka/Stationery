using System.Text.Json.Serialization;

namespace Stationery.Services;

public sealed record LocationOption(
	[property: JsonPropertyName("id")] int Id,
	[property: JsonPropertyName("name")] string Name,
	[property: JsonPropertyName("postalCode")] string? PostalCode = null);
using System.Text.Json.Serialization;

namespace Uk.Legislation.Models.Common;

/// <summary>
/// Generic paged response wrapper
/// </summary>
/// <typeparam name="T">Type of items in the response</typeparam>
public class PagedResponse<T>
{
	/// <summary>
	/// The items in this page
	/// </summary>
	[JsonPropertyName("results")]
	public List<T> Results { get; set; } = [];

	/// <summary>
	/// Total number of results available
	/// </summary>
	[JsonPropertyName("totalResults")]
	public int TotalResults { get; set; }

	/// <summary>
	/// Number of results in this page
	/// </summary>
	[JsonPropertyName("pageSize")]
	public int PageSize { get; set; }

	/// <summary>
	/// Current page number (1-based)
	/// </summary>
	[JsonPropertyName("page")]
	public int Page { get; set; }

	/// <summary>
	/// Total number of pages
	/// </summary>
	[JsonPropertyName("totalPages")]
	public int TotalPages { get; set; }
}

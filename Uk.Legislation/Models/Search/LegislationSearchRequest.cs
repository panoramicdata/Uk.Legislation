using Uk.Legislation.Models.Common;

namespace Uk.Legislation.Models.Search;

/// <summary>
/// Request parameters for searching UK legislation
/// </summary>
public class LegislationSearchRequest
{
	/// <summary>
	/// Free text search query
	/// </summary>
	public string? Text { get; set; }

	/// <summary>
	/// Search within title only
	/// </summary>
	public string? Title { get; set; }

	/// <summary>
	/// Filter by legislation type(s)
	/// </summary>
	public LegislationType[]? Types { get; set; }

	/// <summary>
	/// Filter by year (exact)
	/// </summary>
	public int? Year { get; set; }

	/// <summary>
	/// Filter by start year (inclusive)
	/// </summary>
	public int? YearFrom { get; set; }

	/// <summary>
	/// Filter by end year (inclusive)
	/// </summary>
	public int? YearTo { get; set; }

	/// <summary>
	/// Filter by legislation number
	/// </summary>
	public int? Number { get; set; }

	/// <summary>
	/// Filter by geographical extent
	/// </summary>
	public GeographicalExtent? Extent { get; set; }

	/// <summary>
	/// Page number (1-based)
	/// </summary>
	public int Page { get; set; } = 1;

	/// <summary>
	/// Number of results per page
	/// </summary>
	public int PageSize { get; set; } = 20;

	/// <summary>
	/// Sort order
	/// </summary>
	public SearchSortOrder SortOrder { get; set; } = SearchSortOrder.Relevance;
}

namespace Uk.Legislation.Models.Search;

/// <summary>
/// Sort order options for search results
/// </summary>
public enum SearchSortOrder
{
	/// <summary>
	/// Sort by relevance (default)
	/// </summary>
	Relevance,

	/// <summary>
	/// Sort by title A-Z
	/// </summary>
	TitleAscending,

	/// <summary>
	/// Sort by title Z-A
	/// </summary>
	TitleDescending,

	/// <summary>
	/// Sort by year (oldest first)
	/// </summary>
	YearAscending,

	/// <summary>
	/// Sort by year (newest first)
	/// </summary>
	YearDescending,

	/// <summary>
	/// Sort by legislation type
	/// </summary>
	TypeAscending
}

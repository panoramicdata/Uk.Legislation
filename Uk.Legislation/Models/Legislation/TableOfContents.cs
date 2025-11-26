using System.Text.Json.Serialization;

namespace Uk.Legislation.Models.Legislation;

/// <summary>
/// Table of contents for a piece of legislation
/// </summary>
public class TableOfContents
{
	/// <summary>
	/// URI of the legislation
	/// </summary>
	[JsonPropertyName("uri")]
	public string Uri { get; set; } = string.Empty;

	/// <summary>
	/// Title of the legislation
	/// </summary>
	[JsonPropertyName("title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Top-level structural items (parts, sections, schedules, etc.)
	/// </summary>
	[JsonPropertyName("items")]
	public List<TocItem> Items { get; set; } = [];
}

/// <summary>
/// An item in the table of contents
/// </summary>
public class TocItem
{
	/// <summary>
	/// Type of item (e.g., "part", "section", "schedule")
	/// </summary>
	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// Number or identifier of the item
	/// </summary>
	[JsonPropertyName("number")]
	public string? Number { get; set; }

	/// <summary>
	/// Title or heading of the item
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// URI to access this specific item
	/// </summary>
	[JsonPropertyName("href")]
	public string? Href { get; set; }

	/// <summary>
	/// Child items (subsections, paragraphs, etc.)
	/// </summary>
	[JsonPropertyName("children")]
	public List<TocItem>? Children { get; set; }

	/// <summary>
	/// Whether this item has been repealed
	/// </summary>
	[JsonPropertyName("repealed")]
	public bool IsRepealed { get; set; }

	/// <summary>
	/// Status of this item
	/// </summary>
	[JsonPropertyName("status")]
	public string? Status { get; set; }
}

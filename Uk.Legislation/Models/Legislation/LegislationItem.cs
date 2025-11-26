using System.Text.Json.Serialization;

namespace Uk.Legislation.Models.Legislation;

/// <summary>
/// Represents a piece of UK legislation
/// </summary>
public class LegislationItem
{
	/// <summary>
	/// The legislation type
	/// </summary>
	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// Year the legislation was enacted/made
	/// </summary>
	[JsonPropertyName("year")]
	public int Year { get; set; }

	/// <summary>
	/// Number of the legislation within its year and type
	/// </summary>
	[JsonPropertyName("number")]
	public int Number { get; set; }

	/// <summary>
	/// Full title of the legislation
	/// </summary>
	[JsonPropertyName("title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Short title (if applicable)
	/// </summary>
	[JsonPropertyName("shortTitle")]
	public string? ShortTitle { get; set; }

	/// <summary>
	/// Alternative number (e.g., for Northern Ireland Orders)
	/// </summary>
	[JsonPropertyName("alternativeNumber")]
	public string? AlternativeNumber { get; set; }

	/// <summary>
	/// URI of the legislation
	/// </summary>
	[JsonPropertyName("uri")]
	public string Uri { get; set; } = string.Empty;

	/// <summary>
	/// Date the legislation was enacted/made
	/// </summary>
	[JsonPropertyName("madeDate")]
	public DateOnly? MadeDate { get; set; }

	/// <summary>
	/// Date the legislation was laid before parliament
	/// </summary>
	[JsonPropertyName("laidDate")]
	public DateOnly? LaidDate { get; set; }

	/// <summary>
	/// Date the legislation came into force
	/// </summary>
	[JsonPropertyName("commencementDate")]
	public DateOnly? CommencementDate { get; set; }

	/// <summary>
	/// Geographical extent of the legislation
	/// </summary>
	[JsonPropertyName("extent")]
	public string? Extent { get; set; }

	/// <summary>
	/// Whether this is revised (up-to-date) legislation
	/// </summary>
	[JsonPropertyName("revised")]
	public bool IsRevised { get; set; }

	/// <summary>
	/// Whether this legislation has been modified
	/// </summary>
	[JsonPropertyName("modified")]
	public bool IsModified { get; set; }

	/// <summary>
	/// Whether this legislation has been repealed
	/// </summary>
	[JsonPropertyName("repealed")]
	public bool IsRepealed { get; set; }

	/// <summary>
	/// Whether this legislation is currently in force
	/// </summary>
	[JsonPropertyName("inForce")]
	public bool IsInForce { get; set; }

	/// <summary>
	/// Link to the legislation content
	/// </summary>
	[JsonPropertyName("href")]
	public string? Href { get; set; }

	/// <summary>
	/// Link to the legislation's table of contents
	/// </summary>
	[JsonPropertyName("contentsHref")]
	public string? ContentsHref { get; set; }

	/// <summary>
	/// Link to the legislation's metadata
	/// </summary>
	[JsonPropertyName("metadataHref")]
	public string? MetadataHref { get; set; }
}

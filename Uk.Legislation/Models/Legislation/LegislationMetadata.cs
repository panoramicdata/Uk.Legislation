using System.Text.Json.Serialization;

namespace Uk.Legislation.Models.Legislation;

/// <summary>
/// Metadata about a piece of legislation
/// </summary>
public class LegislationMetadata
{
	/// <summary>
	/// URI of the legislation
	/// </summary>
	[JsonPropertyName("uri")]
	public string Uri { get; set; } = string.Empty;

	/// <summary>
	/// Type of legislation
	/// </summary>
	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// Year enacted/made
	/// </summary>
	[JsonPropertyName("year")]
	public int Year { get; set; }

	/// <summary>
	/// Number
	/// </summary>
	[JsonPropertyName("number")]
	public int Number { get; set; }

	/// <summary>
	/// Full title
	/// </summary>
	[JsonPropertyName("title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Long title (formal title)
	/// </summary>
	[JsonPropertyName("longTitle")]
	public string? LongTitle { get; set; }

	/// <summary>
	/// Short title
	/// </summary>
	[JsonPropertyName("shortTitle")]
	public string? ShortTitle { get; set; }

	/// <summary>
	/// Alternative title
	/// </summary>
	[JsonPropertyName("alternativeTitle")]
	public string? AlternativeTitle { get; set; }

	/// <summary>
	/// Date made
	/// </summary>
	[JsonPropertyName("madeDate")]
	public DateOnly? MadeDate { get; set; }

	/// <summary>
	/// Date laid before parliament
	/// </summary>
	[JsonPropertyName("laidDate")]
	public DateOnly? LaidDate { get; set; }

	/// <summary>
	/// Date coming into force
	/// </summary>
	[JsonPropertyName("commencementDate")]
	public DateOnly? CommencementDate { get; set; }

	/// <summary>
	/// Royal assent date (for Acts)
	/// </summary>
	[JsonPropertyName("royalAssentDate")]
	public DateOnly? RoyalAssentDate { get; set; }

	/// <summary>
	/// Geographical extent
	/// </summary>
	[JsonPropertyName("extent")]
	public string? Extent { get; set; }

	/// <summary>
	/// Subject categories/keywords
	/// </summary>
	[JsonPropertyName("subjects")]
	public List<string> Subjects { get; set; } = [];

	/// <summary>
	/// ISBN (if applicable)
	/// </summary>
	[JsonPropertyName("isbn")]
	public string? Isbn { get; set; }

	/// <summary>
	/// Status (e.g., "Revised", "As Enacted")
	/// </summary>
	[JsonPropertyName("status")]
	public string? Status { get; set; }

	/// <summary>
	/// Publisher
	/// </summary>
	[JsonPropertyName("publisher")]
	public string? Publisher { get; set; }

	/// <summary>
	/// Language (e.g., "en", "cy")
	/// </summary>
	[JsonPropertyName("language")]
	public string? Language { get; set; }

	/// <summary>
	/// Description
	/// </summary>
	[JsonPropertyName("description")]
	public string? Description { get; set; }
}

using System.Text.Json.Serialization;

namespace Uk.Legislation.Models.Legislation;

/// <summary>
/// A specific provision (section, regulation, article, etc.) within legislation
/// </summary>
public class Provision
{
	/// <summary>
	/// URI of this provision
	/// </summary>
	[JsonPropertyName("uri")]
	public string Uri { get; set; } = string.Empty;

	/// <summary>
	/// Type of provision (e.g., "section", "regulation", "article")
	/// </summary>
	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// Number or identifier
	/// </summary>
	[JsonPropertyName("number")]
	public string Number { get; set; } = string.Empty;

	/// <summary>
	/// Title or heading
	/// </summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	/// <summary>
	/// HTML content of the provision
	/// </summary>
	[JsonPropertyName("content")]
	public string? Content { get; set; }

	/// <summary>
	/// Plain text content
	/// </summary>
	[JsonPropertyName("text")]
	public string? Text { get; set; }

	/// <summary>
	/// Whether this provision has been repealed
	/// </summary>
	[JsonPropertyName("repealed")]
	public bool IsRepealed { get; set; }

	/// <summary>
	/// Status information
	/// </summary>
	[JsonPropertyName("status")]
	public string? Status { get; set; }

	/// <summary>
	/// Child provisions (subsections, paragraphs, etc.)
	/// </summary>
	[JsonPropertyName("children")]
	public List<Provision>? Children { get; set; }
}

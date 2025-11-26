namespace Uk.Legislation;

/// <summary>
/// Configuration options for the UK Legislation API client
/// </summary>
public class LegislationClientOptions
{
	/// <summary>
	/// Base URL for the Legislation API
	/// </summary>
	public string BaseUrl { get; set; } = "https://www.legislation.gov.uk";

	/// <summary>
	/// HTTP request timeout
	/// </summary>
	public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

	/// <summary>
	/// User agent string sent with requests
	/// </summary>
	public string UserAgent { get; set; } = $"Uk.Legislation/{ThisAssembly.AssemblyInformationalVersion} (.NET {Environment.Version})";

	/// <summary>
	/// Default format for API responses
	/// </summary>
	public LegislationFormat DefaultFormat { get; set; } = LegislationFormat.Json;

	/// <summary>
	/// Enable response caching (in-memory)
	/// </summary>
	public bool EnableCaching { get; set; } = false;

	/// <summary>
	/// Cache expiration time (only used if EnableCaching is true)
	/// </summary>
	public TimeSpan CacheExpiration { get; set; } = TimeSpan.FromMinutes(5);

	/// <summary>
	/// Maximum number of retry attempts for transient failures
	/// </summary>
	public int MaxRetryAttempts { get; set; } = 3;
}

/// <summary>
/// Supported output formats for legislation content
/// </summary>
public enum LegislationFormat
{
	/// <summary>
	/// JSON format
	/// </summary>
	Json,

	/// <summary>
	/// XML format (CLML - Crown Legislation Markup Language)
	/// </summary>
	Xml,

	/// <summary>
	/// HTML format
	/// </summary>
	Html,

	/// <summary>
	/// RDF/Turtle format (linked data)
	/// </summary>
	Rdf,

	/// <summary>
	/// PDF format (where available)
	/// </summary>
	Pdf
}

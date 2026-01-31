using Refit;
using Uk.Legislation.Models.Common;

namespace Uk.Legislation.Interfaces;

/// <summary>
/// API interface for retrieving UK legislation (XML/Atom formats)
/// </summary>
public interface ILegislationApi
{
	/// <summary>
	/// Get legislation XML by type, year, and number
	/// </summary>
	/// <param name="type">Legislation type</param>
	/// <param name="year">Year</param>
	/// <param name="number">Number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>XML content as string</returns>
	[Get("/{type}/{year}/{number}/data.xml")]
	[Headers("Accept: application/xml")]
	Task<string> GetLegislationXmlAsync(
		LegislationType type,
		int year,
		int number,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get legislation XML at a specific point in time
	/// </summary>
	/// <param name="type">Legislation type</param>
	/// <param name="year">Year</param>
	/// <param name="number">Number</param>
	/// <param name="theDate">The date (YYYY-MM-DD format)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>XML content as string</returns>
	[Get("/{type}/{year}/{number}/{theDate}/data.xml")]
	[Headers("Accept: application/xml")]
	Task<string> GetLegislationAtDateXmlAsync(
		LegislationType type,
		int year,
		int number,
		string theDate,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get legislation as enacted (original version) in XML
	/// </summary>
	/// <param name="type">Legislation type</param>
	/// <param name="year">Year</param>
	/// <param name="number">Number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>XML content as string</returns>
	[Get("/{type}/{year}/{number}/enacted/data.xml")]
	[Headers("Accept: application/xml")]
	Task<string> GetLegislationAsEnactedXmlAsync(
		LegislationType type,
		int year,
		int number,
		CancellationToken cancellationToken);


	/// <summary>
	/// Get a specific provision XML
	/// </summary>
	/// <param name="type">Legislation type</param>
	/// <param name="year">Year</param>
	/// <param name="number">Number</param>
	/// <param name="provisionType">Provision type (e.g., "section", "regulation", "article")</param>
	/// <param name="provisionNumber">Provision number (e.g., "1", "2/a")</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>XML content as string</returns>
	[Get("/{type}/{year}/{number}/{provisionType}/{provisionNumber}/data.xml")]
	[Headers("Accept: application/xml")]
	Task<string> GetProvisionXmlAsync(
		LegislationType type,
		int year,
		int number,
		string provisionType,
		string provisionNumber,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get table of contents XML
	/// </summary>
	/// <param name="type">Legislation type</param>
	/// <param name="year">Year</param>
	/// <param name="number">Number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>XML content as string</returns>
	[Get("/{type}/{year}/{number}/contents/data.xml")]
	[Headers("Accept: application/xml")]
	Task<string> GetTableOfContentsXmlAsync(
		LegislationType type,
		int year,
		int number,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get Atom feed of all legislation of a specific type
	/// </summary>
	/// <param name="type">Legislation type</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Atom feed XML as string</returns>
	[Get("/{type}/data.feed")]
	[Headers("Accept: application/atom+xml")]
	Task<string> GetLegislationByTypeFeedAsync(
		LegislationType type,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get Atom feed of all legislation of a specific type and year
	/// </summary>
	/// <param name="type">Legislation type</param>
	/// <param name="year">Year</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Atom feed XML as string</returns>
	[Get("/{type}/{year}/data.feed")]
	[Headers("Accept: application/atom+xml")]
	Task<string> GetLegislationByTypeAndYearFeedAsync(
		LegislationType type,
		int year,
		CancellationToken cancellationToken);

	/// <summary>
	/// Get legislation HTML (for fallback/debugging)
	/// </summary>
	/// <param name="type">Legislation type</param>
	/// <param name="year">Year</param>
	/// <param name="number">Number</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>HTML content as string</returns>
	[Get("/{type}/{year}/{number}")]
	[Headers("Accept: text/html")]
	Task<string> GetLegislationHtmlAsync(
		LegislationType type,
		int year,
		int number,
		CancellationToken cancellationToken);
}

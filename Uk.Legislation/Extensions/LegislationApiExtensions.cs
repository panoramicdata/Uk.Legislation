using Uk.Legislation.Interfaces;
using Uk.Legislation.Models.Common;
using Uk.Legislation.Models.Legislation;

namespace Uk.Legislation.Extensions;

/// <summary>
/// Extension methods for ILegislationApi to provide convenient typed access
/// </summary>
public static class LegislationApiExtensions
{
	/// <summary>
	/// Get legislation and return as typed object (basic parsing)
	/// </summary>
	public static async Task<LegislationItem> GetLegislationAsync(
		this ILegislationApi api,
		LegislationType type,
		int year,
		int number,
		CancellationToken cancellationToken)
	{
		var xml = await api.GetLegislationXmlAsync(type, year, number, cancellationToken).ConfigureAwait(false);

		// Basic parsing - extract key info from XML
		// Full CLML parsing would be more complex
		return new LegislationItem
		{
			Type = type.ToUriCode(),
			Year = year,
			Number = number,
			Uri = $"/{type.ToUriCode()}/{year}/{number}",
			Title = ExtractTitle(xml) ?? $"{type.ToUriCode().ToUpperInvariant()} {year}/{number}"
		};
	}

	/// <summary>
	/// Get legislation by type with pagination
	/// </summary>
	public static async Task<PagedResponse<LegislationItem>> GetLegislationByTypeAsync(
		this ILegislationApi api,
		LegislationType type,
		CancellationToken cancellationToken)
	{
		var feedXml = await api.GetLegislationByTypeFeedAsync(type, cancellationToken).ConfigureAwait(false);
		return feedXml.ParseAtomFeedPaged();
	}

	/// <summary>
	/// Get legislation by type and year with pagination
	/// </summary>
	public static async Task<PagedResponse<LegislationItem>> GetLegislationByTypeAndYearAsync(
		this ILegislationApi api,
		LegislationType type,
		int year,
		CancellationToken cancellationToken)
	{
		var feedXml = await api.GetLegislationByTypeAndYearFeedAsync(type, year, cancellationToken).ConfigureAwait(false);
		return feedXml.ParseAtomFeedPaged();
	}

	private static string? ExtractTitle(string xml)
	{
		// Quick and dirty title extraction from XML
		// Look for <dc:title> or <ukm:Title> elements
		var titleStart = xml.IndexOf("<dc:title>", StringComparison.OrdinalIgnoreCase);
		if (titleStart == -1)
		{
			titleStart = xml.IndexOf("<ukm:Title>", StringComparison.OrdinalIgnoreCase);
		}

		if (titleStart == -1)
		{
			return null;
		}

		var titleEnd = xml.IndexOf("</", titleStart, StringComparison.OrdinalIgnoreCase);
		if (titleEnd == -1)
		{
			return null;
		}

		var startTag = xml.LastIndexOf('>', titleStart) + 1;
		return xml[startTag..titleEnd];
	}
}

using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;
using Uk.Legislation.Models.Common;
using Uk.Legislation.Models.Legislation;

namespace Uk.Legislation.Extensions;

/// <summary>
/// Extension methods for parsing Atom feeds from legislation.gov.uk
/// </summary>
public static class AtomFeedExtensions
{
	/// <summary>
	/// Parse an Atom feed into a list of legislation items
	/// </summary>
	/// <param name="atomXml">Atom feed XML content</param>
	/// <returns>List of legislation items</returns>
	public static List<LegislationItem> ParseAtomFeed(this string atomXml)
	{
		var items = new List<LegislationItem>();

		using var stringReader = new StringReader(atomXml);
		using var xmlReader = XmlReader.Create(stringReader);

		var feed = SyndicationFeed.Load(xmlReader);

		foreach (var entry in feed.Items)
		{
			var item = CreateLegislationItem(entry);
			items.Add(item);
		}

		return items;
	}

	/// <summary>
	/// Create a legislation item from a syndication entry
	/// </summary>
	private static LegislationItem CreateLegislationItem(SyndicationItem entry)
	{
		var item = new LegislationItem
		{
			Title = entry.Title?.Text ?? string.Empty,
			Uri = entry.Id ?? string.Empty,
			Href = entry.Links.FirstOrDefault()?.Uri.ToString()
		};

		ParseUriComponents(item);
		ExtractDates(entry, item);
		ExtractSummary(entry, item);

		return item;
	}

	/// <summary>
	/// Parse the URI to extract type, year, and number
	/// </summary>
	private static void ParseUriComponents(LegislationItem item)
	{
		if (string.IsNullOrEmpty(item.Uri))
		{
			return;
		}

		// Format is typically: http://www.legislation.gov.uk/{type}/{year}/{number}
		var parts = item.Uri.Split('/', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length < 5)
		{
			return;
		}

		item.Type = parts[3];
		_ = int.TryParse(parts[4], out var year);
		item.Year = year;

		if (parts.Length > 5)
		{
			_ = int.TryParse(parts[5], out var number);
			item.Number = number;
		}
	}

	/// <summary>
	/// Extract dates from syndication entry
	/// </summary>
	private static void ExtractDates(SyndicationItem entry, LegislationItem item)
	{
		if (entry.PublishDate != DateTimeOffset.MinValue)
		{
			item.MadeDate = DateOnly.FromDateTime(entry.PublishDate.DateTime);
		}
	}

	/// <summary>
	/// Extract summary as short title if available
	/// </summary>
	private static void ExtractSummary(SyndicationItem entry, LegislationItem item)
	{
		if (entry.Summary != null)
		{
			item.ShortTitle = entry.Summary.Text;
		}
	}

	/// <summary>
	/// Parse an Atom feed into a paged response
	/// </summary>
	/// <param name="atomXml">Atom feed XML content</param>
	/// <returns>Paged response of legislation items</returns>
	public static PagedResponse<LegislationItem> ParseAtomFeedPaged(this string atomXml)
	{
		var items = ParseAtomFeed(atomXml);
		var doc = XDocument.Parse(atomXml);

		var (total, start, perPage) = ExtractPaginationInfo(doc, items.Count);

		return new PagedResponse<LegislationItem>
		{
			Results = items,
			TotalResults = total,
			PageSize = perPage > 0 ? perPage : items.Count,
			Page = start > 0 && perPage > 0 ? ((start - 1) / perPage) + 1 : 1,
			TotalPages = perPage > 0 && total > 0 ? (int)Math.Ceiling((double)total / perPage) : 1
		};
	}

	/// <summary>
	/// Extract pagination information from the XML document
	/// </summary>
	private static (int total, int start, int perPage) ExtractPaginationInfo(XDocument doc, int itemCount)
	{
		var openSearchNs = XNamespace.Get("http://a9.com/-/spec/opensearch/1.1/");
		var legNs = XNamespace.Get("http://www.legislation.gov.uk/namespaces/legislation");

		var (total, start, perPage) = ParseOpenSearchElements(doc, openSearchNs);

		// Try leg: namespace if OpenSearch didn't give us total
		if (total == 0)
		{
			total = TryEstimateTotalFromLegislationNamespace(doc, legNs, perPage);
		}

		// If still no total, use item count as fallback
		if (total == 0)
		{
			total = itemCount;
		}

		return (total, start, perPage);
	}

	/// <summary>
	/// Parse OpenSearch elements for pagination
	/// </summary>
	private static (int total, int start, int perPage) ParseOpenSearchElements(XDocument doc, XNamespace openSearchNs)
	{
		var totalResults = doc.Descendants(openSearchNs + "totalResults").FirstOrDefault()?.Value;
		var startIndex = doc.Descendants(openSearchNs + "startIndex").FirstOrDefault()?.Value;
		var itemsPerPage = doc.Descendants(openSearchNs + "itemsPerPage").FirstOrDefault()?.Value;

		_ = int.TryParse(totalResults, out var total);
		_ = int.TryParse(startIndex, out var start);
		_ = int.TryParse(itemsPerPage, out var perPage);

		return (total, start, perPage);
	}

	/// <summary>
	/// Try to estimate total results from legislation namespace elements
	/// </summary>
	private static int TryEstimateTotalFromLegislationNamespace(XDocument doc, XNamespace legNs, int perPage)
	{
		var page = doc.Descendants(legNs + "page").FirstOrDefault()?.Value;
		var morePages = doc.Descendants(legNs + "morePages").FirstOrDefault()?.Value;

		_ = int.TryParse(page, out var currentPage);
		_ = int.TryParse(morePages, out var additionalPages);

		// Estimate total based on pages and items per page
		if (perPage > 0 && (currentPage > 0 || additionalPages > 0))
		{
			var totalPages = currentPage + additionalPages;
			return totalPages * perPage;
		}

		return 0;
	}
}

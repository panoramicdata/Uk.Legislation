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
			var item = new LegislationItem
			{
				Title = entry.Title?.Text ?? string.Empty,
				Uri = entry.Id ?? string.Empty,
				Href = entry.Links.FirstOrDefault()?.Uri.ToString()
			};

			// Parse the ID to extract type, year, number
			// Format is typically: http://www.legislation.gov.uk/{type}/{year}/{number}
			if (!string.IsNullOrEmpty(item.Uri))
			{
				var parts = item.Uri.Split('/', StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length >= 5)
				{
					item.Type = parts[3];
					_ = int.TryParse(parts[4], out var year);
					item.Year = year;
					if (parts.Length > 5)
					{
						_ = int.TryParse(parts[5], out var number);
						item.Number = number;
					}
				}
			}

			// Extract dates from content
			if (entry.PublishDate != DateTimeOffset.MinValue)
			{
				item.MadeDate = DateOnly.FromDateTime(entry.PublishDate.DateTime);
			}

			// Extract summary as short title if available
			if (entry.Summary != null)
			{
				item.ShortTitle = entry.Summary.Text;
			}

			items.Add(item);
		}

		return items;
	}

	/// <summary>
	/// Parse an Atom feed into a paged response
	/// </summary>
	/// <param name="atomXml">Atom feed XML content</param>
	/// <returns>Paged response of legislation items</returns>
	public static PagedResponse<LegislationItem> ParseAtomFeedPaged(this string atomXml)
	{
		var items = ParseAtomFeed(atomXml);

		// Try to extract pagination info from OpenSearch or leg: namespaces
		var doc = XDocument.Parse(atomXml);
		var openSearchNs = XNamespace.Get("http://a9.com/-/spec/opensearch/1.1/");
		var legNs = XNamespace.Get("http://www.legislation.gov.uk/namespaces/legislation");

		// Try OpenSearch elements first
		var totalResults = doc.Descendants(openSearchNs + "totalResults").FirstOrDefault()?.Value;
		var startIndex = doc.Descendants(openSearchNs + "startIndex").FirstOrDefault()?.Value;
		var itemsPerPage = doc.Descendants(openSearchNs + "itemsPerPage").FirstOrDefault()?.Value;

		_ = int.TryParse(totalResults, out var total);
		_ = int.TryParse(startIndex, out var start);
		_ = int.TryParse(itemsPerPage, out var perPage);

		// Try leg: namespace if OpenSearch didn't give us total
		if (total == 0)
		{
			var page = doc.Descendants(legNs + "page").FirstOrDefault()?.Value;
			var morePages = doc.Descendants(legNs + "morePages").FirstOrDefault()?.Value;

			_ = int.TryParse(page, out var currentPage);
			_ = int.TryParse(morePages, out var additionalPages);

			// Estimate total based on pages and items per page
			if (perPage > 0 && (currentPage > 0 || additionalPages > 0))
			{
				var totalPages = currentPage + additionalPages;
				total = totalPages * perPage;
			}
		}

		// If still no total, use item count as fallback
		if (total == 0)
		{
			total = items.Count;
		}

		return new PagedResponse<LegislationItem>
		{
			Results = items,
			TotalResults = total,
			PageSize = perPage > 0 ? perPage : items.Count,
			Page = start > 0 && perPage > 0 ? ((start - 1) / perPage) + 1 : 1,
			TotalPages = perPage > 0 && total > 0 ? (int)Math.Ceiling((double)total / perPage) : 1
		};
	}
}

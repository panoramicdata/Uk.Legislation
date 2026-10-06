using AwesomeAssertions;
using Uk.Legislation.Extensions;
using static Uk.Legislation.Test.UnitTests.AtomFeedBuilder;

namespace Uk.Legislation.Test.UnitTests;

/// <summary>
/// Unit tests for <see cref="AtomFeedExtensions"/>
/// </summary>
[Trait("Category", "Unit")]
public class AtomFeedExtensionsTests
{
	/// <summary>
	/// Verifies an entry's URI, dates, summary and link are parsed.
	/// </summary>
	[Fact]
	public void ParseAtomFeed_WithFullEntry_ParsesAllFields()
	{
		// Arrange
		var feed = Feed(string.Empty, Entry(
			"uksi/2021/1234",
			"The Test Regulations 2021",
			"<summary>Test Regulations</summary>" +
			"<published>2021-03-15T00:00:00Z</published>" +
			"<link href=\"http://www.legislation.gov.uk/uksi/2021/1234\"/>"));

		// Act
		var items = feed.ParseAtomFeed();

		// Assert
		var item = items.Should().ContainSingle().Subject;
		_ = item.Title.Should().Be("The Test Regulations 2021");
		_ = item.Uri.Should().Be(IdBase + "uksi/2021/1234");
		_ = item.Href.Should().Be("http://www.legislation.gov.uk/uksi/2021/1234");
		_ = item.Type.Should().Be("uksi");
		_ = item.Year.Should().Be(2021);
		_ = item.Number.Should().Be(1234);
		_ = item.ShortTitle.Should().Be("Test Regulations");
		_ = item.MadeDate.Should().Be(new DateOnly(2021, 3, 15));
	}

	/// <summary>
	/// Verifies a URI with type and year but no number leaves the number unset.
	/// </summary>
	[Fact]
	public void ParseAtomFeed_WithUriWithoutNumber_ParsesTypeAndYearOnly()
	{
		// Act
		var item = Feed(string.Empty, Entry("ukpga/2020", "Acts of 2020")).ParseAtomFeed().Single();

		// Assert
		_ = item.Type.Should().Be("ukpga");
		_ = item.Year.Should().Be(2020);
		_ = item.Number.Should().Be(0);
		_ = item.Href.Should().BeNull();
		_ = item.ShortTitle.Should().BeNull();
		_ = item.MadeDate.Should().BeNull();
	}

	/// <summary>
	/// Verifies short or missing URIs and titles do not throw.
	/// </summary>
	[Fact]
	public void ParseAtomFeed_WithShortOrMissingUri_LeavesComponentsUnset()
	{
		// Act
		var items = Feed(string.Empty, Entry("ukpga", "Short"), "<entry></entry>").ParseAtomFeed();

		// Assert
		_ = items.Should().HaveCount(2);
		_ = items[0].Type.Should().BeEmpty();
		_ = items[0].Year.Should().Be(0);
		_ = items[1].Uri.Should().BeEmpty();
		_ = items[1].Title.Should().BeEmpty();
	}

	/// <summary>
	/// Verifies OpenSearch pagination elements drive the paged response.
	/// </summary>
	[Fact]
	public void ParseAtomFeedPaged_WithOpenSearchElements_CalculatesPaging()
	{
		// Arrange
		var feed = Feed(
			OpenSearch("totalResults", 45) + OpenSearch("startIndex", 21) + OpenSearch("itemsPerPage", 20),
			Entry("ukpga/2020/21", "Act 21"));

		// Act
		var result = feed.ParseAtomFeedPaged();

		// Assert
		_ = result.TotalResults.Should().Be(45);
		_ = result.PageSize.Should().Be(20);
		_ = result.Page.Should().Be(2);
		_ = result.TotalPages.Should().Be(3);
		_ = result.Results.Should().ContainSingle();
	}

	/// <summary>
	/// Verifies the total is estimated from legislation namespace paging when OpenSearch gives none.
	/// </summary>
	[Fact]
	public void ParseAtomFeedPaged_WithLegislationNamespacePaging_EstimatesTotal()
	{
		// Arrange
		var feed = Feed(
			OpenSearch("itemsPerPage", 20) + Leg("page", 2) + Leg("morePages", 3),
			Entry("ukpga/2020/1", "Act 1"));

		// Act
		var result = feed.ParseAtomFeedPaged();

		// Assert
		_ = result.TotalResults.Should().Be(100);
		_ = result.TotalPages.Should().Be(5);
		_ = result.Page.Should().Be(1);
	}

	/// <summary>
	/// Verifies the item count is used when the feed has no paging information.
	/// </summary>
	[Fact]
	public void ParseAtomFeedPaged_WithoutPagingInformation_FallsBackToItemCount()
	{
		// Arrange
		var feed = Feed(string.Empty, Entry("ukpga/2020/1", "Act 1"), Entry("ukpga/2020/2", "Act 2"));

		// Act
		var result = feed.ParseAtomFeedPaged();

		// Assert
		_ = result.TotalResults.Should().Be(2);
		_ = result.PageSize.Should().Be(2);
		_ = result.Page.Should().Be(1);
		_ = result.TotalPages.Should().Be(1);
	}
}

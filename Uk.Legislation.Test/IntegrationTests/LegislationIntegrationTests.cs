using AwesomeAssertions;
using Uk.Legislation.Extensions;
using Uk.Legislation.Models.Common;

namespace Uk.Legislation.Test.IntegrationTests;

/// <summary>
/// Integration tests for legislation retrieval (requires live API)
/// </summary>
[Trait("Category", "Integration")]
public class LegislationIntegrationTests : IntegrationTestBase
{
	/// <summary>
	/// Verifies XML can be retrieved for a known Act.
	/// </summary>
	[Fact]
	public async Task GetLegislationXmlAsync_WithKnownAct_ReturnsXml()
	{
		// Arrange - Use a well-known Act: Human Rights Act 1998
		const LegislationType type = LegislationType.UkPublicGeneralAct;
		const int year = 1998;
		const int number = 42;

		// Act
		var result = await Client.Legislation.GetLegislationXmlAsync(type, year, number, CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<Legislation");
		_ = result.Should().Contain("Human Rights Act");
	}

	/// <summary>
	/// Verifies parsed legislation can be retrieved for a known Act.
	/// </summary>
	[Fact]
	public async Task GetLegislationAsync_WithKnownAct_ReturnsLegislation()
	{
		// Arrange - Use a well-known Act: Human Rights Act 1998
		const LegislationType type = LegislationType.UkPublicGeneralAct;
		const int year = 1998;
		const int number = 42;

		// Act
		var result = await Client.Legislation.GetLegislationAsync(type, year, number, CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Type.Should().Be("ukpga");
		_ = result.Year.Should().Be(year);
		_ = result.Number.Should().Be(number);
		_ = result.Title.Should().NotBeNullOrWhiteSpace();
	}

	/// <summary>
	/// Verifies date-versioned XML can be retrieved for known legislation.
	/// </summary>
	[Fact]
	public async Task GetLegislationAtDateXmlAsync_WithValidDate_ReturnsXml()
	{
		// Arrange - Get Human Rights Act 1998 as it was on 1 Jan 2020
		const LegislationType type = LegislationType.UkPublicGeneralAct;
		const int year = 1998;
		const int number = 42;
		const string date = "2020-01-01";

		// Act
		var result = await Client.Legislation.GetLegislationAtDateXmlAsync(type, year, number, date, CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<Legislation");
	}

	/// <summary>
	/// Verifies as-enacted XML can be retrieved for a known Act.
	/// </summary>
	[Fact]
	public async Task GetLegislationAsEnactedXmlAsync_WithKnownAct_ReturnsXml()
	{
		// Arrange - Get Human Rights Act 1998 as enacted
		const LegislationType type = LegislationType.UkPublicGeneralAct;
		const int year = 1998;
		const int number = 42;

		// Act
		var result = await Client.Legislation.GetLegislationAsEnactedXmlAsync(type, year, number, CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<Legislation");
	}

	/// <summary>
	/// Verifies provision XML can be retrieved for a known section.
	/// </summary>
	[Fact]
	public async Task GetProvisionXmlAsync_WithKnownSection_ReturnsXml()
	{
		// Arrange - Get section 1 of Human Rights Act 1998
		const LegislationType type = LegislationType.UkPublicGeneralAct;
		const int year = 1998;
		const int number = 42;

		// Act
		var result = await Client.Legislation.GetProvisionXmlAsync(type, year, number, "section", "1", CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<Legislation");
	}

	/// <summary>
	/// Verifies table-of-contents XML can be retrieved for a known Act.
	/// </summary>
	[Fact]
	public async Task GetTableOfContentsXmlAsync_WithKnownAct_ReturnsXml()
	{
		// Arrange - Get ToC for Human Rights Act 1998
		const LegislationType type = LegislationType.UkPublicGeneralAct;
		const int year = 1998;
		const int number = 42;

		// Act
		var result = await Client.Legislation.GetTableOfContentsXmlAsync(type, year, number, CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<ContentsTitle");
	}

	/// <summary>
	/// Verifies Atom feed XML can be retrieved for a legislation type.
	/// </summary>
	[Fact]
	public async Task GetLegislationByTypeFeedAsync_WithValidType_ReturnsAtomFeed()
	{
		// Arrange
		const LegislationType type = LegislationType.UkPublicGeneralAct;

		// Act
		var result = await Client.Legislation.GetLegislationByTypeFeedAsync(type, CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<feed");
		_ = result.Should().Contain("http://www.w3.org/2005/Atom");
	}

	/// <summary>
	/// Verifies Atom feed XML can be retrieved for a legislation type and year.
	/// </summary>
	[Fact]
	public async Task GetLegislationByTypeAndYearFeedAsync_WithValidParameters_ReturnsAtomFeed()
	{
		// Arrange
		const LegislationType type = LegislationType.UkPublicGeneralAct;
		const int year = 1998;

		// Act
		var result = await Client.Legislation.GetLegislationByTypeAndYearFeedAsync(type, year, CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<feed");
	}

	/// <summary>
	/// Verifies parsed results can be retrieved for a legislation type.
	/// </summary>
	[Fact]
	public async Task GetLegislationByTypeAsync_WithValidType_ReturnsParsedResults()
	{
		// Arrange
		const LegislationType type = LegislationType.UkPublicGeneralAct;

		// Act
		var result = await Client.Legislation.GetLegislationByTypeAsync(type, CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Results.Should().NotBeEmpty();
		_ = result.TotalResults.Should().BePositive();
	}

	/// <summary>
	/// Verifies parsed results can be retrieved for a legislation type and year.
	/// </summary>
	[Fact]
	public async Task GetLegislationByTypeAndYearAsync_WithValidParameters_ReturnsParsedResults()
	{
		// Arrange
		const LegislationType type = LegislationType.UkPublicGeneralAct;
		const int year = 1998;

		// Act
		var result = await Client.Legislation.GetLegislationByTypeAndYearAsync(type, year, CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Results.Should().NotBeEmpty();
		_ = result.Results.Should().Contain(item => item.Year == year);
	}

	/// <summary>
	/// Verifies Atom feed parsing extracts legislation items from a real feed.
	/// </summary>
	[Fact]
	public async Task ParseAtomFeed_WithRealFeed_ExtractsLegislationItems()
	{
		// Arrange
		var feedXml = await Client.Legislation.GetLegislationByTypeAndYearFeedAsync(
			LegislationType.UkPublicGeneralAct,
			1998,
			CancellationToken);

		// Act
		var items = feedXml.ParseAtomFeed();

		// Assert
		_ = items.Should().NotBeEmpty();
		_ = items.Should().AllSatisfy(item =>
		{
			_ = item.Title.Should().NotBeNullOrWhiteSpace();
			_ = item.Uri.Should().NotBeNullOrWhiteSpace();
		});
	}

	/// <summary>
	/// Verifies HTML can be retrieved for a known Act.
	/// </summary>
	[Fact]
	public async Task GetLegislationHtmlAsync_WithKnownAct_ReturnsHtml()
	{
		// Arrange
		const LegislationType type = LegislationType.UkPublicGeneralAct;
		const int year = 1998;
		const int number = 42;

		// Act
		var result = await Client.Legislation.GetLegislationHtmlAsync(type, year, number, CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<html");
		_ = result.Should().Contain("Human Rights Act");
	}
}

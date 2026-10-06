using AwesomeAssertions;
using Moq;
using Uk.Legislation.Extensions;
using Uk.Legislation.Interfaces;
using Uk.Legislation.Models.Common;

namespace Uk.Legislation.Test.UnitTests;

/// <summary>
/// Unit tests for <see cref="LegislationApiExtensions"/>
/// </summary>
[Trait("Category", "Unit")]
public class LegislationApiExtensionsTests
{
	private static CancellationToken CancellationToken => TestContext.Current.CancellationToken;

	/// <summary>
	/// Verifies the title is extracted from a ukm:Title element when there is no dc:title.
	/// </summary>
	[Fact]
	public async Task GetLegislationAsync_WithUkmTitle_ExtractsTitle()
	{
		// Arrange
		var api = MockXml("<Legislation><ukm:Title>The Test Order 2019</ukm:Title></Legislation>");

		// Act
		var result = await api.GetLegislationAsync(LegislationType.UkStatutoryInstrument, 2019, 7, CancellationToken);

		// Assert
		_ = result.Title.Should().Be("The Test Order 2019");
		_ = result.Uri.Should().Be("/uksi/2019/7");
	}

	/// <summary>
	/// Verifies a fallback title is generated when the XML has no title element.
	/// </summary>
	[Fact]
	public async Task GetLegislationAsync_WithoutTitle_UsesFallbackTitle()
	{
		// Arrange
		var api = MockXml("<Legislation><Body/></Legislation>");

		// Act
		var result = await api.GetLegislationAsync(LegislationType.UkPublicGeneralAct, 2020, 1, CancellationToken);

		// Assert
		_ = result.Title.Should().Be("UKPGA 2020/1");
	}

	/// <summary>
	/// Verifies a fallback title is generated when the title element is never closed.
	/// </summary>
	[Fact]
	public async Task GetLegislationAsync_WithUnterminatedTitle_UsesFallbackTitle()
	{
		// Arrange
		var api = MockXml("<dc:title>Unterminated");

		// Act
		var result = await api.GetLegislationAsync(LegislationType.UkPublicGeneralAct, 2020, 2, CancellationToken);

		// Assert
		_ = result.Title.Should().Be("UKPGA 2020/2");
	}

	/// <summary>
	/// Verifies the type-and-year feed is parsed into a paged response.
	/// </summary>
	[Fact]
	public async Task GetLegislationByTypeAndYearAsync_WithFeed_ReturnsPagedResponse()
	{
		// Arrange
		var feed = AtomFeedBuilder.Feed(string.Empty, AtomFeedBuilder.Entry("ukpga/2020/1", "Act 1"));
		var api = new Mock<ILegislationApi>();
		_ = api
			.Setup(a => a.GetLegislationByTypeAndYearFeedAsync(LegislationType.UkPublicGeneralAct, 2020, It.IsAny<CancellationToken>()))
			.ReturnsAsync(feed);

		// Act
		var result = await api.Object.GetLegislationByTypeAndYearAsync(LegislationType.UkPublicGeneralAct, 2020, CancellationToken);

		// Assert
		var item = result.Results.Should().ContainSingle().Subject;
		_ = item.Year.Should().Be(2020);
		_ = item.Number.Should().Be(1);
	}

	private static ILegislationApi MockXml(string xml)
	{
		var api = new Mock<ILegislationApi>();
		_ = api
			.Setup(a => a.GetLegislationXmlAsync(It.IsAny<LegislationType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(xml);
		return api.Object;
	}
}

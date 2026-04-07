using AwesomeAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using Uk.Legislation.Extensions;
using Uk.Legislation.Models.Common;

namespace Uk.Legislation.Test.IntegrationTests;

/// <summary>
/// Unit tests for the Legislation API
/// </summary>
[Trait("Category", "Unit")]
public class LegislationApiUnitTests : IntegrationTestBase
{
	/// <summary>
	/// Verifies XML is returned for a valid legislation request.
	/// </summary>
	[Fact]
	public async Task GetLegislationXmlAsync_WithValidParameters_ReturnsXml()
	{
		// Arrange
		const string mockXml = "<?xml version=\"1.0\"?><legislation><title>Test Act 2020</title></legislation>";
		var client = CreateMockClient(mockXml);

		// Act
		var result = await client
			.Legislation
			.GetLegislationXmlAsync(LegislationType.UkPublicGeneralAct, 2020, 1, CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<?xml");
		_ = result.Should().Contain("Test Act 2020");
	}

	/// <summary>
	/// Verifies parsed legislation is returned for a valid legislation request.
	/// </summary>
	[Fact]
	public async Task GetLegislationAsync_WithValidParameters_ReturnsParsedLegislation()
	{
		// Arrange
		const string mockXml = "<?xml version=\"1.0\"?><legislation><dc:title>Test Act 2020</dc:title></legislation>";
		var client = CreateMockClient(mockXml);

		// Act
		var result = await client
			.Legislation
			.GetLegislationAsync(
				LegislationType.UkPublicGeneralAct,
				2020,
				1,
				CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Type.Should().Be("ukpga");
		_ = result.Year.Should().Be(2020);
		_ = result.Number.Should().Be(1);
		_ = result.Title.Should().NotBeNullOrWhiteSpace();
	}

	/// <summary>
	/// Verifies XML is returned for legislation at a specific date.
	/// </summary>
	[Fact]
	public async Task GetLegislationAtDateXmlAsync_WithValidParameters_ReturnsXml()
	{
		// Arrange
		const string mockXml = "<?xml version=\"1.0\"?><legislation><title>Test Act 2020 as at 2021-01-01</title></legislation>";
		var client = CreateMockClient(mockXml);

		// Act
		var result = await client.Legislation.GetLegislationAtDateXmlAsync(
			LegislationType.UkPublicGeneralAct, 2020, 1, "2021-01-01", CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<?xml");
	}

	/// <summary>
	/// Verifies XML is returned for legislation as enacted.
	/// </summary>
	[Fact]
	public async Task GetLegislationAsEnactedXmlAsync_WithValidParameters_ReturnsXml()
	{
		// Arrange
		const string mockXml = "<?xml version=\"1.0\"?><legislation><title>Test Act 2020 (as enacted)</title></legislation>";
		var client = CreateMockClient(mockXml);

		// Act
		var result = await client.Legislation.GetLegislationAsEnactedXmlAsync(LegislationType.UkPublicGeneralAct, 2020, 1, CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<?xml");
	}

	/// <summary>
	/// Verifies XML is returned for a valid provision request.
	/// </summary>
	[Fact]
	public async Task GetProvisionXmlAsync_WithValidParameters_ReturnsXml()
	{
		// Arrange
		const string mockXml = "<?xml version=\"1.0\"?><section><num>1</num><heading>Test Section</heading></section>";
		var client = CreateMockClient(mockXml);

		// Act
		var result = await client.Legislation.GetProvisionXmlAsync(LegislationType.UkPublicGeneralAct, 2020, 1, "section", "1", CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<?xml");
		_ = result.Should().Contain("section");
	}

	/// <summary>
	/// Verifies an Atom feed is returned for a valid legislation type.
	/// </summary>
	[Fact]
	public async Task GetLegislationByTypeFeedAsync_WithValidType_ReturnsAtomFeed()
	{
		// Arrange
		const string mockFeed = """
			<?xml version="1.0"?>
			<feed xmlns="http://www.w3.org/2005/Atom">
				<entry>
					<id>http://www.legislation.gov.uk/ukpga/2020/1</id>
					<title>Test Act 2020</title>
				</entry>
			</feed>
			""";
		var client = CreateMockClient(mockFeed);

		// Act
		var result = await client.Legislation.GetLegislationByTypeFeedAsync(LegislationType.UkPublicGeneralAct, CancellationToken);

		// Assert
		_ = result.Should().NotBeNullOrWhiteSpace();
		_ = result.Should().Contain("<feed");
		_ = result.Should().Contain("http://www.w3.org/2005/Atom");
	}

	/// <summary>
	/// Verifies parsed results are returned for legislation by type.
	/// </summary>
	[Fact]
	public async Task GetLegislationByTypeAsync_WithValidType_ReturnsNonNullResult()
	{
		// Arrange
		var client = CreateMockClient(GetMockAtomFeed());

		// Act
		var result = await client
			.Legislation
			.GetLegislationByTypeAsync(
				LegislationType.UkPublicGeneralAct,
				cancellationToken: CancellationToken);

		// Assert
		_ = result.Should().NotBeNull();
		_ = result.Results.Should().NotBeEmpty();
	}

	/// <summary>
	/// Verifies total result count is parsed correctly for legislation by type.
	/// </summary>
	[Fact]
	public async Task GetLegislationByTypeAsync_WithValidType_ReturnsCorrectTotalResults()
	{
		// Arrange
		var client = CreateMockClient(GetMockAtomFeed());

		// Act
		var result = await client
			.Legislation
			.GetLegislationByTypeAsync(
				LegislationType.UkPublicGeneralAct,
				cancellationToken: CancellationToken);

		// Assert
		_ = result.TotalResults.Should().Be(2);
	}

	/// <summary>
	/// Verifies item count is parsed correctly for legislation by type.
	/// </summary>
	[Fact]
	public async Task GetLegislationByTypeAsync_WithValidType_ReturnsCorrectItemCount()
	{
		// Arrange
		var client = CreateMockClient(GetMockAtomFeed());

		// Act
		var result = await client
			.Legislation
			.GetLegislationByTypeAsync(
				LegislationType.UkPublicGeneralAct,
				cancellationToken: CancellationToken);

		// Assert
		_ = result.Results.Should().HaveCount(2);
	}

	private static string GetMockAtomFeed() => """
		<?xml version="1.0"?>
		<feed xmlns="http://www.w3.org/2005/Atom" xmlns:openSearch="http://a9.com/-/spec/opensearch/1.1/">
			<openSearch:totalResults>2</openSearch:totalResults>
			<openSearch:startIndex>0</openSearch:startIndex>
			<openSearch:itemsPerPage>20</openSearch:itemsPerPage>
			<entry>
				<id>http://www.legislation.gov.uk/ukpga/2020/1</id>
				<title>Test Act 2020 No. 1</title>
				<link href="http://www.legislation.gov.uk/ukpga/2020/1"/>
			</entry>
			<entry>
				<id>http://www.legislation.gov.uk/ukpga/2020/2</id>
				<title>Test Act 2020 No. 2</title>
				<link href="http://www.legislation.gov.uk/ukpga/2020/2"/>
			</entry>
		</feed>
		""";

	/// <summary>
	/// Verifies the default constructor creates a usable client.
	/// </summary>
	[Fact]
	public void Constructor_WithNullOptions_UsesDefaults()
	{
		// Act
		using var client = new LegislationClient();

		// Assert
		_ = client.Should().NotBeNull();
		_ = client.Legislation.Should().NotBeNull();
	}

	/// <summary>
	/// Verifies custom options are accepted by the client constructor.
	/// </summary>
	[Fact]
	public void Constructor_WithCustomOptions_UsesProvidedOptions()
	{
		// Arrange
		var options = new LegislationClientOptions
		{
			BaseUrl = "https://custom.legislation.gov.uk",
			Timeout = TimeSpan.FromSeconds(60)
		};

		// Act
		using var client = new LegislationClient(options);

		// Assert
		_ = client.Should().NotBeNull();
		_ = client.Legislation.Should().NotBeNull();
	}

	/// <summary>
	/// Verifies legislation types map to the expected URI codes.
	/// </summary>
	[Fact]
	public void LegislationTypeExtensions_ToUriCode_ReturnsCorrectCode()
	{
		// Act & Assert
		_ = LegislationType.UkPublicGeneralAct.ToUriCode().Should().Be("ukpga");
		_ = LegislationType.UkStatutoryInstrument.ToUriCode().Should().Be("uksi");
		_ = LegislationType.ScottishAct.ToUriCode().Should().Be("asp");
		_ = LegislationType.WalesStatutoryInstrument.ToUriCode().Should().Be("wsi");
	}

	/// <summary>
	/// Verifies URI codes map to the expected legislation types.
	/// </summary>
	[Fact]
	public void LegislationTypeExtensions_FromUriCode_ReturnsCorrectEnum()
	{
		// Act & Assert
		_ = LegislationTypeExtensions.FromUriCode("ukpga").Should().Be(LegislationType.UkPublicGeneralAct);
		_ = LegislationTypeExtensions.FromUriCode("uksi").Should().Be(LegislationType.UkStatutoryInstrument);
		_ = LegislationTypeExtensions.FromUriCode("asp").Should().Be(LegislationType.ScottishAct);
	}

	/// <summary>
	/// Verifies an invalid URI code throws an exception.
	/// </summary>
	[Fact]
	public void LegislationTypeExtensions_FromUriCode_ThrowsOnInvalidCode()
	{
		// Act
		var act = () => LegislationTypeExtensions.FromUriCode("invalid");

		// Assert
		_ = act.Should().Throw<ArgumentException>();
	}

	/// <summary>
	/// Verifies TryFromUriCode succeeds for a valid URI code.
	/// </summary>
	[Fact]
	public void LegislationTypeExtensions_TryFromUriCode_ReturnsTrueForValidCode()
	{
		// Act
		var success = LegislationTypeExtensions.TryFromUriCode("ukpga", out var type);

		// Assert
		_ = success.Should().BeTrue();
		_ = type.Should().Be(LegislationType.UkPublicGeneralAct);
	}

	/// <summary>
	/// Verifies TryFromUriCode fails for an invalid URI code.
	/// </summary>
	[Fact]
	public void LegislationTypeExtensions_TryFromUriCode_ReturnsFalseForInvalidCode()
	{
		// Act
		var success = LegislationTypeExtensions.TryFromUriCode("invalid", out var type);

		// Assert
		_ = success.Should().BeFalse();
		_ = type.Should().Be(default);
	}

	private static LegislationClient CreateMockClient(string responseContent)
	{
		var mockHandler = new Mock<HttpMessageHandler>();

		_ = mockHandler.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(responseContent, System.Text.Encoding.UTF8, "application/xml")
			});

		var httpClient = new HttpClient(mockHandler.Object)
		{
			BaseAddress = new Uri("https://www.legislation.gov.uk")
		};

		return new LegislationClient(httpClient);
	}
}

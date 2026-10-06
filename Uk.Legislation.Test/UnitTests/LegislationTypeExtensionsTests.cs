using AwesomeAssertions;
using Uk.Legislation.Extensions;
using Uk.Legislation.Models.Common;
using Uk.Legislation.Models.Legislation;
using Uk.Legislation.Models.Search;

namespace Uk.Legislation.Test.UnitTests;

/// <summary>
/// Unit tests for <see cref="LegislationTypeExtensions"/> edge cases and model defaults
/// </summary>
[Trait("Category", "Unit")]
public class LegislationTypeExtensionsTests
{
	/// <summary>
	/// Verifies an undefined enum value falls back to its lower-cased name.
	/// </summary>
	[Fact]
	public void ToUriCode_WithUndefinedValue_ReturnsLowerCasedName() =>
		_ = ((LegislationType)999).ToUriCode().Should().Be("999");

	/// <summary>
	/// Verifies blank URI codes are rejected.
	/// </summary>
	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	public void FromUriCode_WithBlankCode_Throws(string uriCode) =>
		_ = FluentActions.Invoking(() => LegislationTypeExtensions.FromUriCode(uriCode))
			.Should().Throw<ArgumentException>()
			.WithParameterName(nameof(uriCode));

	/// <summary>
	/// Verifies URI codes are matched case-insensitively.
	/// </summary>
	[Fact]
	public void FromUriCode_WithUpperCaseCode_ReturnsEnum() =>
		_ = LegislationTypeExtensions.FromUriCode("UKPGA").Should().Be(LegislationType.UkPublicGeneralAct);

	/// <summary>
	/// Verifies TryFromUriCode fails for null or blank codes.
	/// </summary>
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void TryFromUriCode_WithBlankCode_ReturnsFalse(string? uriCode)
	{
		// Act
		var success = LegislationTypeExtensions.TryFromUriCode(uriCode, out var type);

		// Assert
		_ = success.Should().BeFalse();
		_ = type.Should().Be(default);
	}

	/// <summary>
	/// Verifies every defined legislation type round-trips through its URI code.
	/// </summary>
	[Fact]
	public void ToUriCode_ForEveryType_RoundTrips()
	{
		foreach (var type in Enum.GetValues<LegislationType>())
		{
			_ = LegislationTypeExtensions.FromUriCode(type.ToUriCode()).Should().Be(type);
		}
	}

	/// <summary>
	/// Verifies the model types initialise with their documented defaults.
	/// </summary>
	[Fact]
	public void Models_WhenConstructed_HaveDefaults()
	{
		var search = new LegislationSearchRequest();
		_ = search.Page.Should().Be(1);
		_ = search.PageSize.Should().Be(20);
		_ = search.SortOrder.Should().Be(SearchSortOrder.Relevance);

		var metadata = new LegislationMetadata();
		_ = metadata.Uri.Should().BeEmpty();
		_ = metadata.Type.Should().BeEmpty();
		_ = metadata.Title.Should().BeEmpty();
		_ = metadata.Subjects.Should().BeEmpty();

		var provision = new Provision();
		_ = provision.Uri.Should().BeEmpty();
		_ = provision.Type.Should().BeEmpty();
		_ = provision.Number.Should().BeEmpty();

		var toc = new TableOfContents();
		_ = toc.Uri.Should().BeEmpty();
		_ = toc.Title.Should().BeEmpty();
		_ = toc.Items.Should().BeEmpty();

		_ = new TocItem().Type.Should().BeEmpty();
	}
}

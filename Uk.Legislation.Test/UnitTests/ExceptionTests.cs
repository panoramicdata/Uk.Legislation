using AwesomeAssertions;
using System.Net;
using Uk.Legislation.Exceptions;

namespace Uk.Legislation.Test.UnitTests;

/// <summary>
/// Unit tests for the exception types
/// </summary>
[Trait("Category", "Unit")]
public class ExceptionTests
{
	private static readonly InvalidOperationException Inner = new("inner");

	/// <summary>
	/// Verifies the <see cref="LegislationApiException"/> constructors.
	/// </summary>
	[Fact]
	public void LegislationApiException_Constructors_SetProperties()
	{
		_ = new LegislationApiException().Message.Should().NotBeNullOrEmpty();
		_ = new LegislationApiException("failed").Message.Should().Be("failed");

		var withInner = new LegislationApiException("failed", Inner);
		_ = withInner.Message.Should().Be("failed");
		_ = withInner.InnerException.Should().BeSameAs(Inner);
	}

	/// <summary>
	/// Verifies the <see cref="HttpStatusResponseException"/> constructors.
	/// </summary>
	[Fact]
	public void HttpStatusResponseException_Constructors_SetProperties()
	{
		var basic = new HttpStatusResponseException(HttpStatusCode.BadRequest, "bad");
		_ = basic.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		_ = basic.Message.Should().Be("bad");
		_ = basic.ResponseContent.Should().BeNull();

		var withContent = new HttpStatusResponseException(HttpStatusCode.InternalServerError, "error", "body");
		_ = withContent.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
		_ = withContent.ResponseContent.Should().Be("body");

		var withInner = new HttpStatusResponseException(HttpStatusCode.BadGateway, "gateway", Inner);
		_ = withInner.StatusCode.Should().Be(HttpStatusCode.BadGateway);
		_ = withInner.InnerException.Should().BeSameAs(Inner);
		_ = withInner.Should().BeAssignableTo<LegislationApiException>();
	}

	/// <summary>
	/// Verifies the <see cref="InvalidLegislationUriException"/> constructors.
	/// </summary>
	[Fact]
	public void InvalidLegislationUriException_Constructors_SetProperties()
	{
		_ = new InvalidLegislationUriException().InvalidUri.Should().BeNull();
		_ = new InvalidLegislationUriException("invalid").Message.Should().Be("invalid");

		var withUri = new InvalidLegislationUriException("invalid", "/bad/uri");
		_ = withUri.Message.Should().Be("invalid");
		_ = withUri.InvalidUri.Should().Be("/bad/uri");

		var withInner = new InvalidLegislationUriException("invalid", Inner);
		_ = withInner.InnerException.Should().BeSameAs(Inner);
		_ = withInner.InvalidUri.Should().BeNull();
	}

	/// <summary>
	/// Verifies the <see cref="LegislationNotFoundException"/> constructors.
	/// </summary>
	[Fact]
	public void LegislationNotFoundException_Constructors_SetProperties()
	{
		_ = new LegislationNotFoundException().LegislationUri.Should().BeNull();
		_ = new LegislationNotFoundException("missing").Message.Should().Be("missing");

		var withUri = new LegislationNotFoundException("missing", "/ukpga/2020/999");
		_ = withUri.Message.Should().Be("missing");
		_ = withUri.LegislationUri.Should().Be("/ukpga/2020/999");

		var withInner = new LegislationNotFoundException("missing", Inner);
		_ = withInner.InnerException.Should().BeSameAs(Inner);
		_ = withInner.LegislationUri.Should().BeNull();
	}
}

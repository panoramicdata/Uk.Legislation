using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;
using Uk.Legislation.Extensions;

namespace Uk.Legislation.Test.UnitTests;

/// <summary>
/// Unit tests for <see cref="ServiceCollectionExtensions"/>
/// </summary>
[Trait("Category", "Unit")]
public class ServiceCollectionExtensionsTests
{
	/// <summary>
	/// Verifies the client resolves with default options.
	/// </summary>
	[Fact]
	public void AddUkLegislationClient_WithDefaults_ResolvesClient()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act
		_ = services.AddUkLegislationClient();
		using var provider = services.BuildServiceProvider();
		using var scope = provider.CreateScope();
		var client = scope.ServiceProvider.GetRequiredService<LegislationClient>();

		// Assert
		_ = client.Legislation.Should().NotBeNull();
	}

	/// <summary>
	/// Verifies configured options are applied to the named HttpClient.
	/// </summary>
	[Fact]
	public void AddUkLegislationClient_WithConfiguration_AppliesOptionsToHttpClient()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act
		_ = services.AddUkLegislationClient(options =>
		{
			options.Timeout = TimeSpan.FromSeconds(42);
			options.UserAgent = "Uk.Legislation.Test/1.0";
		});
		using var provider = services.BuildServiceProvider();
		var httpClient = provider
			.GetRequiredService<IHttpClientFactory>()
			.CreateClient(nameof(LegislationClient));

		// Assert
		_ = httpClient.Timeout.Should().Be(TimeSpan.FromSeconds(42));
		_ = httpClient.DefaultRequestHeaders.UserAgent.ToString().Should().Be("Uk.Legislation.Test/1.0");
		_ = httpClient.DefaultRequestHeaders.Accept.Should().ContainSingle(h => h.MediaType == "application/json");
	}

	/// <summary>
	/// Verifies the client resolves when resilience policies are added.
	/// </summary>
	[Fact]
	public void AddUkLegislationClientWithResilience_WithDefaults_ResolvesClient()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act
		_ = services.AddUkLegislationClientWithResilience();
		using var provider = services.BuildServiceProvider();
		using var scope = provider.CreateScope();
		var client = scope.ServiceProvider.GetRequiredService<LegislationClient>();

		// Assert
		_ = client.Legislation.Should().NotBeNull();
	}

	/// <summary>
	/// Verifies null arguments are rejected.
	/// </summary>
	[Fact]
	public void AddUkLegislationClient_WithNullArguments_Throws()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act & Assert
		_ = FluentActions.Invoking(() => ServiceCollectionExtensions.AddUkLegislationClient(null!, _ => { }))
			.Should().Throw<ArgumentNullException>();
		_ = FluentActions.Invoking(() => services.AddUkLegislationClient(null!))
			.Should().Throw<ArgumentNullException>();
		_ = FluentActions.Invoking(() => ServiceCollectionExtensions.AddUkLegislationClientWithResilience(null!, _ => { }))
			.Should().Throw<ArgumentNullException>();
		_ = FluentActions.Invoking(() => services.AddUkLegislationClientWithResilience(null!))
			.Should().Throw<ArgumentNullException>();
	}
}

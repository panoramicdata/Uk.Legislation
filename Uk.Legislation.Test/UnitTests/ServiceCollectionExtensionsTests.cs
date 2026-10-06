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
	/// The registration methods under test, each applied with default options.
	/// </summary>
	public static TheoryData<string> Registrations =>
	[
		nameof(ServiceCollectionExtensions.AddUkLegislationClient),
		nameof(ServiceCollectionExtensions.AddUkLegislationClientWithResilience),
	];

	/// <summary>
	/// Verifies each registration method lets a scope resolve a usable client.
	/// </summary>
	[Theory]
	[MemberData(nameof(Registrations))]
	public void Register_WithDefaults_ResolvesClient(string registration)
	{
		// Arrange
		var services = new ServiceCollection();
		_ = Register(registration, services, static _ => { });

		// Act
		using var provider = services.BuildServiceProvider();
		using var scope = provider.CreateScope();
		var client = scope.ServiceProvider.GetRequiredService<LegislationClient>();

		// Assert
		_ = client.Legislation.Should().NotBeNull();
	}

	/// <summary>
	/// Verifies each registration method rejects null arguments.
	/// </summary>
	[Theory]
	[MemberData(nameof(Registrations))]
	public void Register_WithNullArguments_Throws(string registration)
	{
		_ = FluentActions.Invoking(() => Register(registration, null!, static _ => { }))
			.Should().Throw<ArgumentNullException>().WithParameterName("services");
		_ = FluentActions.Invoking(() => Register(registration, new ServiceCollection(), null!))
			.Should().Throw<ArgumentNullException>().WithParameterName("configure");
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
	/// Verifies the parameterless overloads register the client.
	/// </summary>
	[Fact]
	public void ParameterlessOverloads_RegisterClient()
	{
		_ = new ServiceCollection().AddUkLegislationClient()
			.Should().Contain(d => d.ServiceType == typeof(LegislationClient));
		_ = new ServiceCollection().AddUkLegislationClientWithResilience()
			.Should().Contain(d => d.ServiceType == typeof(LegislationClient));
	}

	private static IServiceCollection Register(
		string registration,
		IServiceCollection services,
		Action<LegislationClientOptions> configure) => registration switch
		{
			nameof(ServiceCollectionExtensions.AddUkLegislationClient) => services.AddUkLegislationClient(configure),
			_ => services.AddUkLegislationClientWithResilience(configure)
		};
}

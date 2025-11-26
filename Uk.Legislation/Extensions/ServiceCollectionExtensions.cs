using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Uk.Legislation.Extensions;

/// <summary>
/// Extension methods for registering UK Legislation API clients with dependency injection
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds the UK Legislation API client to the service collection
	/// </summary>
	/// <param name="services">The service collection</param>
	/// <param name="configure">Optional callback to configure client options</param>
	/// <returns>The service collection for chaining</returns>
	public static IServiceCollection AddUkLegislationClient(
		this IServiceCollection services,
		Action<LegislationClientOptions>? configure = null)
	{
		// Register options
		if (configure is not null)
		{
			_ = services.Configure(configure);
		}
		else
		{
			_ = services.AddOptions<LegislationClientOptions>();
		}

		// Register HttpClient for LegislationClient
		_ = services.AddHttpClient<LegislationClient>((serviceProvider, httpClient) =>
		{
			var options = serviceProvider.GetService<IOptions<LegislationClientOptions>>()?.Value
				?? new LegislationClientOptions();

			httpClient.Timeout = options.Timeout;
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(
				new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		});

		// Register LegislationClient as scoped
		_ = services.AddScoped<LegislationClient>(serviceProvider =>
		{
			var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
			var httpClient = httpClientFactory.CreateClient(nameof(LegislationClient));
			var options = serviceProvider.GetService<IOptions<LegislationClientOptions>>()?.Value;

			return new LegislationClient(httpClient, options);
		});

		return services;
	}

	/// <summary>
	/// Adds the UK Legislation API client with Polly resilience policies
	/// </summary>
	/// <param name="services">The service collection</param>
	/// <param name="configure">Optional callback to configure client options</param>
	/// <returns>The service collection for chaining</returns>
	public static IServiceCollection AddUkLegislationClientWithResilience(
		this IServiceCollection services,
		Action<LegislationClientOptions>? configure = null)
	{
		_ = services.AddUkLegislationClient(configure);

		// Add Polly policies to the HttpClient
		_ = services.AddHttpClient<LegislationClient>()
			.AddPolicyHandler(HttpPolicyExtensions
				.HandleTransientHttpError()
				.WaitAndRetryAsync(3, retryAttempt =>
					TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
			.AddPolicyHandler(HttpPolicyExtensions
				.HandleTransientHttpError()
				.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

		return services;
	}
}

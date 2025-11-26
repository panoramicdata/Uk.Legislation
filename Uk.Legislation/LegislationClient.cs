using Refit;
using Uk.Legislation.Converters;
using Uk.Legislation.Interfaces;

namespace Uk.Legislation;

/// <summary>
/// Main client for accessing the UK Legislation API
/// </summary>
public class LegislationClient : IDisposable
{
	private readonly HttpClient? _ownedHttpClient;
	private readonly LegislationClientOptions _options;
	private readonly bool _disposeHttpClient;

	/// <summary>
	/// Legislation retrieval API (returns raw XML/Atom content)
	/// </summary>
	public ILegislationApi Legislation { get; }

	/// <summary>
	/// Constructor with options (creates own HttpClient)
	/// </summary>
	/// <param name="options">Configuration options</param>
	public LegislationClient(LegislationClientOptions? options = null)
	{
		_options = options ?? new LegislationClientOptions();
		_ownedHttpClient = new HttpClient
		{
			Timeout = _options.Timeout
		};
		_disposeHttpClient = true;

		ConfigureHttpClient(_ownedHttpClient);

		var refitSettings = GetRefitSettings();
		Legislation = RestService.For<ILegislationApi>(_ownedHttpClient, refitSettings);
	}

	/// <summary>
	/// Constructor with custom HttpClient (for dependency injection)
	/// </summary>
	/// <param name="httpClient">Pre-configured HttpClient</param>
	/// <param name="options">Configuration options</param>
	public LegislationClient(HttpClient httpClient, LegislationClientOptions? options = null)
	{
		ArgumentNullException.ThrowIfNull(httpClient);

		_options = options ?? new LegislationClientOptions();
		_disposeHttpClient = false;

		ConfigureHttpClient(httpClient);

		var refitSettings = GetRefitSettings();
		Legislation = RestService.For<ILegislationApi>(httpClient, refitSettings);
	}

	private void ConfigureHttpClient(HttpClient httpClient)
	{
		httpClient.BaseAddress = new Uri(_options.BaseUrl);
		httpClient.DefaultRequestHeaders.UserAgent.Clear();
		httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_options.UserAgent);
	}

	private static RefitSettings GetRefitSettings() => new()
	{
		UrlParameterFormatter = new LegislationTypeUrlParameterFormatter()
	};

	/// <summary>
	/// Disposes the client and underlying HttpClient (if owned)
	/// </summary>
	public void Dispose()
	{
		if (_disposeHttpClient)
		{
			_ownedHttpClient?.Dispose();
		}

		GC.SuppressFinalize(this);
	}
}

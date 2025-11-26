namespace Uk.Legislation.Test.IntegrationTests;

/// <summary>
/// Base class for integration tests that interact with the live legislation.gov.uk API
/// </summary>
public abstract class IntegrationTestBase : IDisposable
{
	protected static CancellationToken CancellationToken => TestContext.Current.CancellationToken;

	/// <summary>
	/// The legislation client for testing
	/// </summary>
	protected LegislationClient Client { get; }

	/// <summary>
	/// Client options for integration tests
	/// </summary>
	protected LegislationClientOptions Options { get; }

	/// <summary>
	/// Constructor
	/// </summary>
	protected IntegrationTestBase()
	{
		Options = new LegislationClientOptions
		{
			BaseUrl = "https://www.legislation.gov.uk",
			Timeout = TimeSpan.FromSeconds(30),
			UserAgent = "Uk.Legislation.Test/10.0.0 (Integration Tests)"
		};

		Client = new LegislationClient(Options);
	}

	/// <summary>
	/// Dispose the client
	/// </summary>
	public void Dispose()
	{
		Client?.Dispose();
		GC.SuppressFinalize(this);
	}
}

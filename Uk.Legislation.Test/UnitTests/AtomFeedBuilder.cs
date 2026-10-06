namespace Uk.Legislation.Test.UnitTests;

/// <summary>
/// Builds Atom feed XML for unit tests
/// </summary>
internal static class AtomFeedBuilder
{
	private const string FeedStart =
		"<?xml version=\"1.0\"?>" +
		"<feed xmlns=\"http://www.w3.org/2005/Atom\"" +
		" xmlns:openSearch=\"http://a9.com/-/spec/opensearch/1.1/\"" +
		" xmlns:leg=\"http://www.legislation.gov.uk/namespaces/legislation\">";

	/// <summary>
	/// The base of legislation.gov.uk feed entry IDs
	/// </summary>
	public const string IdBase = "http://www.legislation.gov.uk/id/";

	/// <summary>
	/// Creates a feed from header elements and entries
	/// </summary>
	public static string Feed(string header, params string[] entries) =>
		FeedStart + header + string.Concat(entries) + "</feed>";

	/// <summary>
	/// Creates a feed entry whose ID is <see cref="IdBase"/> followed by <paramref name="path"/>
	/// </summary>
	public static string Entry(string path, string title, string extra = "") =>
		$"<entry><id>{IdBase}{path}</id><title>{title}</title>{extra}</entry>";

	/// <summary>
	/// Creates an OpenSearch element
	/// </summary>
	public static string OpenSearch(string name, int value) =>
		$"<openSearch:{name}>{value}</openSearch:{name}>";

	/// <summary>
	/// Creates a legislation namespace element
	/// </summary>
	public static string Leg(string name, int value) =>
		$"<leg:{name}>{value}</leg:{name}>";
}

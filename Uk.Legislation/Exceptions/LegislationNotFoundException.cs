namespace Uk.Legislation.Exceptions;

/// <summary>
/// Exception thrown when requested legislation is not found
/// </summary>
public class LegislationNotFoundException : LegislationApiException
{
	/// <summary>
	/// Gets the legislation URI that was not found
	/// </summary>
	public string? LegislationUri { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="LegislationNotFoundException"/> class
	/// </summary>
	public LegislationNotFoundException()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="LegislationNotFoundException"/> class with a message
	/// </summary>
	/// <param name="message">The error message</param>
	public LegislationNotFoundException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="LegislationNotFoundException"/> class with a message and URI
	/// </summary>
	/// <param name="message">The error message</param>
	/// <param name="legislationUri">The URI of the legislation that was not found</param>
	public LegislationNotFoundException(string message, string legislationUri)
		: base(message)
	{
		LegislationUri = legislationUri;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="LegislationNotFoundException"/> class with a message and inner exception
	/// </summary>
	/// <param name="message">The error message</param>
	/// <param name="innerException">The inner exception</param>
	public LegislationNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}

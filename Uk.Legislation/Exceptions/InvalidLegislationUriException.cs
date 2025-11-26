namespace Uk.Legislation.Exceptions;

/// <summary>
/// Exception thrown when a legislation URI is invalid or malformed
/// </summary>
public class InvalidLegislationUriException : LegislationApiException
{
	/// <summary>
	/// Gets the invalid URI that was provided
	/// </summary>
	public string? InvalidUri { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidLegislationUriException"/> class
	/// </summary>
	public InvalidLegislationUriException()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidLegislationUriException"/> class with a message
	/// </summary>
	/// <param name="message">The error message</param>
	public InvalidLegislationUriException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidLegislationUriException"/> class with a message and URI
	/// </summary>
	/// <param name="message">The error message</param>
	/// <param name="invalidUri">The invalid URI that was provided</param>
	public InvalidLegislationUriException(string message, string invalidUri)
		: base(message)
	{
		InvalidUri = invalidUri;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidLegislationUriException"/> class with a message and inner exception
	/// </summary>
	/// <param name="message">The error message</param>
	/// <param name="innerException">The inner exception</param>
	public InvalidLegislationUriException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}

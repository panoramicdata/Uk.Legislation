namespace Uk.Legislation.Exceptions;

/// <summary>
/// Base exception for all UK Legislation API errors
/// </summary>
public class LegislationApiException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="LegislationApiException"/> class
	/// </summary>
	public LegislationApiException()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="LegislationApiException"/> class with a message
	/// </summary>
	/// <param name="message">The error message</param>
	public LegislationApiException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="LegislationApiException"/> class with a message and inner exception
	/// </summary>
	/// <param name="message">The error message</param>
	/// <param name="innerException">The inner exception</param>
	public LegislationApiException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}

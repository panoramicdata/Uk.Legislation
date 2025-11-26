using System.Net;

namespace Uk.Legislation.Exceptions;

/// <summary>
/// Exception thrown when an HTTP request fails with a specific status code
/// </summary>
public class HttpStatusResponseException : LegislationApiException
{
	/// <summary>
	/// Gets the HTTP status code from the response
	/// </summary>
	public HttpStatusCode StatusCode { get; }

	/// <summary>
	/// Gets the response content (if available)
	/// </summary>
	public string? ResponseContent { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="HttpStatusResponseException"/> class
	/// </summary>
	/// <param name="statusCode">The HTTP status code</param>
	/// <param name="message">The error message</param>
	public HttpStatusResponseException(HttpStatusCode statusCode, string message)
		: base(message)
	{
		StatusCode = statusCode;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HttpStatusResponseException"/> class
	/// </summary>
	/// <param name="statusCode">The HTTP status code</param>
	/// <param name="message">The error message</param>
	/// <param name="responseContent">The response content</param>
	public HttpStatusResponseException(HttpStatusCode statusCode, string message, string responseContent)
		: base(message)
	{
		StatusCode = statusCode;
		ResponseContent = responseContent;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HttpStatusResponseException"/> class
	/// </summary>
	/// <param name="statusCode">The HTTP status code</param>
	/// <param name="message">The error message</param>
	/// <param name="innerException">The inner exception</param>
	public HttpStatusResponseException(HttpStatusCode statusCode, string message, Exception innerException)
		: base(message, innerException)
	{
		StatusCode = statusCode;
	}
}

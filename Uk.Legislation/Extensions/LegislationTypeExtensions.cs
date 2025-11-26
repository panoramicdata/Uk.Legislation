using System.ComponentModel;
using System.Reflection;
using Uk.Legislation.Models.Common;

namespace Uk.Legislation.Extensions;

/// <summary>
/// Extension methods for LegislationType enum
/// </summary>
public static class LegislationTypeExtensions
{
	/// <summary>
	/// Converts a LegislationType enum value to its URI code (e.g., UkPublicGeneralAct -> "ukpga")
	/// </summary>
	/// <param name="type">The legislation type</param>
	/// <returns>URI code string</returns>
	public static string ToUriCode(this LegislationType type)
	{
		var field = type.GetType().GetField(type.ToString());
		if (field is null)
		{
			return type.ToString().ToLowerInvariant();
		}

		var attr = field.GetCustomAttribute<DescriptionAttribute>();
		return attr?.Description ?? type.ToString().ToLowerInvariant();
	}

	/// <summary>
	/// Converts a URI code string to a LegislationType enum value (e.g., "ukpga" -> UkPublicGeneralAct)
	/// </summary>
	/// <param name="uriCode">The URI code (e.g., "ukpga", "uksi")</param>
	/// <returns>LegislationType enum value</returns>
	/// <exception cref="ArgumentException">Thrown when the URI code is not recognized</exception>
	public static LegislationType FromUriCode(string uriCode)
	{
		if (string.IsNullOrWhiteSpace(uriCode))
		{
			throw new ArgumentException("URI code cannot be null or empty", nameof(uriCode));
		}

		// Try to find enum value by Description attribute
		foreach (var value in Enum.GetValues<LegislationType>())
		{
			if (value.ToUriCode().Equals(uriCode, StringComparison.OrdinalIgnoreCase))
			{
				return value;
			}
		}

		throw new ArgumentException($"Unknown legislation type URI code: {uriCode}", nameof(uriCode));
	}

	/// <summary>
	/// Tries to convert a URI code string to a LegislationType enum value
	/// </summary>
	/// <param name="uriCode">The URI code</param>
	/// <param name="type">The resulting legislation type if successful</param>
	/// <returns>True if conversion succeeded, false otherwise</returns>
	public static bool TryFromUriCode(string? uriCode, out LegislationType type)
	{
		type = default;

		if (string.IsNullOrWhiteSpace(uriCode))
		{
			return false;
		}

		try
		{
			type = FromUriCode(uriCode);
			return true;
		}
		catch
		{
			return false;
		}
	}
}

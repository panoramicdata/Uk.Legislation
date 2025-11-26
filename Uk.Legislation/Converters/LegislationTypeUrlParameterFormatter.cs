using Refit;
using System.Reflection;
using Uk.Legislation.Extensions;
using Uk.Legislation.Models.Common;

namespace Uk.Legislation.Converters;

/// <summary>
/// URL parameter formatter for LegislationType enum that converts to URI codes
/// </summary>
public class LegislationTypeUrlParameterFormatter : IUrlParameterFormatter
{
	/// <summary>
	/// Formats the LegislationType enum value as a URI code for URL parameters
	/// </summary>
	public string? Format(object? value, ICustomAttributeProvider attributeProvider, Type type)
			=> value is LegislationType legislationType
				? legislationType.ToUriCode()
				: (value?.ToString());
}

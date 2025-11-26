using System.ComponentModel;

namespace Uk.Legislation.Models.Common;

/// <summary>
/// Geographical extent (territorial application) of legislation
/// </summary>
[Flags]
public enum GeographicalExtent
{
	/// <summary>
	/// No extent specified
	/// </summary>
	[Description("")]
	None = 0,

	/// <summary>
	/// Applies to England
	/// </summary>
	[Description("E")]
	England = 1,

	/// <summary>
	/// Applies to Wales
	/// </summary>
	[Description("W")]
	Wales = 2,

	/// <summary>
	/// Applies to Scotland
	/// </summary>
	[Description("S")]
	Scotland = 4,

	/// <summary>
	/// Applies to Northern Ireland
	/// </summary>
	[Description("NI")]
	NorthernIreland = 8,

	/// <summary>
	/// Applies to England and Wales
	/// </summary>
	[Description("E+W")]
	EnglandWales = England | Wales,

	/// <summary>
	/// Applies to England, Wales and Scotland
	/// </summary>
	[Description("E+W+S")]
	GreatBritain = England | Wales | Scotland,

	/// <summary>
	/// Applies to all of the United Kingdom
	/// </summary>
	[Description("E+W+S+NI")]
	UnitedKingdom = England | Wales | Scotland | NorthernIreland
}

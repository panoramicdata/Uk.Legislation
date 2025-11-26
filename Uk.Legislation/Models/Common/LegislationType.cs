using System.ComponentModel;

namespace Uk.Legislation.Models.Common;

/// <summary>
/// Types of UK legislation available through the API
/// </summary>
public enum LegislationType
{
	/// <summary>
	/// UK Public General Acts
	/// </summary>
	[Description("ukpga")]
	UkPublicGeneralAct,

	/// <summary>
	/// UK Local Acts
	/// </summary>
	[Description("ukla")]
	UkLocalAct,

	/// <summary>
	/// UK Private and Personal Acts
	/// </summary>
	[Description("ukppa")]
	UkPrivateAct,

	/// <summary>
	/// Acts of the Scottish Parliament
	/// </summary>
	[Description("asp")]
	ScottishAct,

	/// <summary>
	/// Acts of Senedd Cymru (Welsh Parliament)
	/// </summary>
	[Description("asc")]
	SeneddAct,

	/// <summary>
	/// Acts of the National Assembly for Wales (historical)
	/// </summary>
	[Description("anaw")]
	WelshAssemblyAct,

	/// <summary>
	/// Measures of the National Assembly for Wales
	/// </summary>
	[Description("mwa")]
	WelshAssemblyMeasure,

	/// <summary>
	/// Church Measures
	/// </summary>
	[Description("ukcm")]
	ChurchMeasure,

	/// <summary>
	/// Acts of the Northern Ireland Assembly
	/// </summary>
	[Description("nia")]
	NorthernIrelandAct,

	/// <summary>
	/// Acts of the Old Scottish Parliament (pre-1707)
	/// </summary>
	[Description("aosp")]
	OldScottishParliamentAct,

	/// <summary>
	/// Acts of the English Parliament (pre-1707)
	/// </summary>
	[Description("aep")]
	EnglishParliamentAct,

	/// <summary>
	/// Acts of the Old Irish Parliament (pre-1800)
	/// </summary>
	[Description("aip")]
	IrishParliamentAct,

	/// <summary>
	/// Acts of the Parliament of Great Britain (1707-1800)
	/// </summary>
	[Description("apgb")]
	GreatBritainAct,

	/// <summary>
	/// Local Acts of the Parliament of Great Britain
	/// </summary>
	[Description("gbla")]
	GreatBritainLocalAct,

	/// <summary>
	/// Private and Personal Acts of the Parliament of Great Britain
	/// </summary>
	[Description("gbppa")]
	GreatBritainPrivateAct,

	/// <summary>
	/// Northern Ireland Orders in Council
	/// </summary>
	[Description("nisi")]
	NorthernIrelandOrderInCouncil,

	/// <summary>
	/// Measures of the Northern Ireland Assembly
	/// </summary>
	[Description("mnia")]
	NorthernIrelandMeasure,

	/// <summary>
	/// Acts of the Northern Ireland Parliament (historical)
	/// </summary>
	[Description("apni")]
	NorthernIrelandParliamentAct,

	/// <summary>
	/// UK Statutory Instruments
	/// </summary>
	[Description("uksi")]
	UkStatutoryInstrument,

	/// <summary>
	/// Wales Statutory Instruments
	/// </summary>
	[Description("wsi")]
	WalesStatutoryInstrument,

	/// <summary>
	/// Scottish Statutory Instruments
	/// </summary>
	[Description("ssi")]
	ScottishStatutoryInstrument,

	/// <summary>
	/// Northern Ireland Statutory Rules
	/// </summary>
	[Description("nisr")]
	NorthernIrelandStatutoryRule,

	/// <summary>
	/// Church Instruments
	/// </summary>
	[Description("ukci")]
	ChurchInstrument,

	/// <summary>
	/// UK Ministerial Directions
	/// </summary>
	[Description("ukmd")]
	UkMinisterialDirection,

	/// <summary>
	/// UK Ministerial Orders
	/// </summary>
	[Description("ukmo")]
	UkMinisterialOrder,

	/// <summary>
	/// UK Statutory Rules and Orders (historical)
	/// </summary>
	[Description("uksro")]
	UkStatutoryRulesAndOrders,

	/// <summary>
	/// Northern Ireland Statutory Rules and Orders (historical)
	/// </summary>
	[Description("nisro")]
	NorthernIrelandStatutoryRulesAndOrders,

	/// <summary>
	/// European Union Regulations
	/// </summary>
	[Description("eur")]
	EuRegulation,

	/// <summary>
	/// European Union Decisions
	/// </summary>
	[Description("eudn")]
	EuDecision,

	/// <summary>
	/// European Union Directives
	/// </summary>
	[Description("eudr")]
	EuDirective,

	/// <summary>
	/// UK Draft Statutory Instruments
	/// </summary>
	[Description("ukdsi")]
	UkDraftStatutoryInstrument,

	/// <summary>
	/// Scottish Draft Statutory Instruments
	/// </summary>
	[Description("sdsi")]
	ScottishDraftStatutoryInstrument,

	/// <summary>
	/// Northern Ireland Draft Statutory Rules
	/// </summary>
	[Description("nidsr")]
	NorthernIrelandDraftStatutoryRule,

	/// <summary>
	/// UK Impact Assessments
	/// </summary>
	[Description("ukia")]
	UkImpactAssessment
}

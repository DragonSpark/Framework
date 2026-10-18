using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Security.Identity;

sealed class WellKnownApplicationSchemes : Instances<string>
{
	public static WellKnownApplicationSchemes Default { get; } = new();

	WellKnownApplicationSchemes() : base("Identity.Application", "EasyAuth") {}
}
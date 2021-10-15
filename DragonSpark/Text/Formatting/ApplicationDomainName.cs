using System;

namespace DragonSpark.Text.Formatting;

sealed class ApplicationDomainName : FormatEntry<AppDomain>
{
	public static ApplicationDomainName Default { get; } = new ApplicationDomainName();

	ApplicationDomainName() : base("F", x => x.FriendlyName) {}
}
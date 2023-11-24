using System;

namespace DragonSpark.Text.Formatting;

sealed class ApplicationDomainIdentifier : FormatEntry<AppDomain>
{
	public static ApplicationDomainIdentifier Default { get; } = new();

	ApplicationDomainIdentifier() : base("I", x => x.Id.ToString()) {}
}
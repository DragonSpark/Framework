using System;

namespace DragonSpark.Text.Formatting;

sealed class ApplicationDomainFormatter : Selection<AppDomain, string>, ISelectFormatter<AppDomain>
{
	public static ApplicationDomainFormatter Default { get; } = new ApplicationDomainFormatter();

	ApplicationDomainFormatter()
		: base(DefaultApplicationDomainFormatter.Default,
		       ApplicationDomainName.Default, ApplicationDomainIdentifier.Default) {}
}
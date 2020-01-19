using DragonSpark.Text;
using DragonSpark.Text.Formatting;
using System;

namespace DragonSpark.Testing.Objects
{
	sealed class ApplicationDomainFormatter : Selection<AppDomain, string>, ISelectFormatter<AppDomain>
	{
		public static ApplicationDomainFormatter Default { get; } = new ApplicationDomainFormatter();

		ApplicationDomainFormatter()
			: base(DefaultApplicationDomainFormatter.Default,
			       ApplicationDomainName.Default,
			       ApplicationDomainIdentifier.Default) {}
	}
}
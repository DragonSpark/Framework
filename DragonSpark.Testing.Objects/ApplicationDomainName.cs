using System;
using DragonSpark.Text.Formatting;

namespace DragonSpark.Testing.Objects
{
	sealed class ApplicationDomainName : FormatEntry<AppDomain>
	{
		public static ApplicationDomainName Default { get; } = new ApplicationDomainName();

		ApplicationDomainName() : base("F", x => x.FriendlyName) {}
	}
}
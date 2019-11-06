using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Text.Formatting
{
	sealed class DefaultApplicationDomainFormatter : Select<AppDomain, string>, IFormatter<AppDomain>
	{
		public static DefaultApplicationDomainFormatter Default { get; } = new DefaultApplicationDomainFormatter();

		DefaultApplicationDomainFormatter() : base(x => $"AppDomain: {x.FriendlyName}") {}
	}
}
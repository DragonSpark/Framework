using DragonSpark.Text;
using DragonSpark.Text.Formatting;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Testing.Objects;

sealed class ApplicationDomainFormatter : Selection<AppDomain, string>, ISelectFormatter<AppDomain>
{
	[UsedImplicitly]
	public static ApplicationDomainFormatter Default { get; } = new();

	ApplicationDomainFormatter()
		: base(DefaultApplicationDomainFormatter.Default, ApplicationDomainName.Default,
		       ApplicationDomainIdentifier.Default) {}
}
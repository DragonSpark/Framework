using DragonSpark.Text.Formatting;
using System;

namespace DragonSpark.Testing.Objects;

sealed class ApplicationDomainIdentifier : FormatEntry<AppDomain>
{
	public static ApplicationDomainIdentifier Default { get; } = new();

	ApplicationDomainIdentifier() : base("I", x => x.Id.ToString()) {}
}
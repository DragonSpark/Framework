using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;
using System;

namespace DragonSpark.Application.Runtime;

public sealed class StandardUniqueIdentityFormatter : IFormatter<Guid>
{
	public static StandardUniqueIdentityFormatter Default { get; } = new();

	StandardUniqueIdentityFormatter() : this(StandardUniqueIdentity.Default) {}

	readonly IAlteration<Guid> _previous;

	public StandardUniqueIdentityFormatter(IAlteration<Guid> previous) => _previous = previous;

	public string Get(Guid parameter) => _previous.Get(parameter).ToString().ToLower();
}
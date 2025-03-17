using DragonSpark.Runtime;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Application.AspNet.Entities;

sealed class EnlistedScopes : IEnlistedScopes
{
	readonly IScopes         _previous;
	readonly IAmbientContext _context;
	readonly IDisposable     _empty;

	public EnlistedScopes(IScopes previous, IAmbientContext context)
		: this(previous, context, EmptyDisposable.Default) {}

	public EnlistedScopes(IScopes previous, IAmbientContext context, IDisposable empty)
	{
		_previous = previous;
		_context  = context;
		_empty    = empty;
	}

	[MustDisposeResource]
	public Scope Get()
	{
		var enlisted = _context.Get();
		return enlisted is not null ? new(enlisted, _empty) : _previous.Get();
	}
}
using DragonSpark.Runtime;

namespace DragonSpark.Application.AspNet.Entities;

public sealed class EnlistedScopes : IEnlistedScopes
{
	readonly IScopes       _previous;
	readonly IAmbientContext _context;

	public EnlistedScopes(IScopes previous, IAmbientContext context)
	{
		_previous = previous;
		_context  = context;
	}

	public Scope Get()
	{
		var enlisted = _context.Get();
		return enlisted is not null ? new Scope(enlisted, EmptyDisposable.Default) : _previous.Get();
	}
}
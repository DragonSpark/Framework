using DragonSpark.Runtime;

namespace DragonSpark.Application.Entities;

public sealed class EnlistedContexts : IEnlistedContexts
{
	readonly IContexts       _previous;
	readonly IAmbientContext _context;

	public EnlistedContexts(IContexts previous, IAmbientContext context)
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
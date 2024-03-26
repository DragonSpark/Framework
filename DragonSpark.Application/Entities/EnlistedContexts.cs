using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public sealed class EnlistedContexts : IEnlistedContexts
{
	readonly IContexts         _previous;
	readonly IAmbientContext _context;

	public EnlistedContexts(IContexts previous, IAmbientContext context)
	{
		_previous = previous;
		_context  = context;
	}

	public DbContext Get() => _context.Get() ?? _previous.Get();
}
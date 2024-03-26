using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public class AmbientAwareScopes : IEnlistedScopes
{
	readonly IScopes         _previous;
	readonly IAmbientContext _context;

	protected AmbientAwareScopes(IScopes previous, IAmbientContext context)
	{
		_previous = previous;
		_context  = context;
	}

	public DbContext Get() => _context.Get() ?? _previous.Get();
}
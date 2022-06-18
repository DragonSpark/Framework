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

	public Scope Get()
	{
		var context = _context.Get();
		var result  = context != null ? new Scope(context, EmptyBoundary.Default) : _previous.Get();
		return result;
	}
}
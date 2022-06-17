using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Presentation.Connections;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class ContextAwareInitializeConnection : IInitializeConnection
{
	readonly ContextStore             _store;
	readonly IInitializeConnection    _previous;
	readonly IAlteration<HttpContext> _context;

	public ContextAwareInitializeConnection(IInitializeConnection previous, ContextStore store)
		: this(previous, store, CloneHttpContext.Default) {}

	public ContextAwareInitializeConnection(IInitializeConnection previous, ContextStore store,
	                                        IAlteration<HttpContext> context)
	{
		_store    = store;
		_previous = previous;
		_context  = context;
	}

	public void Execute(HttpContext parameter)
	{
		_previous.Execute(parameter);
		var clone = _context.Get(parameter);
		_store.Execute(clone);
	}
}
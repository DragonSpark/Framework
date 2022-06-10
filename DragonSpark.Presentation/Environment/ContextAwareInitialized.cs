using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Presentation.Connections.Initialization;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class ContextAwareInitialized : IInitialized
{
	readonly ContextStore             _store;
	readonly IInitialized             _previous;
	readonly IAlteration<HttpContext> _context;

	public ContextAwareInitialized(ContextStore store, IInitialized previous)
		: this(store, previous, CloneHttpContext.Default) {}

	public ContextAwareInitialized(ContextStore store, IInitialized previous, IAlteration<HttpContext> context)
	{
		_store    = store;
		_previous = previous;
		_context  = context;
	}

	public bool Get(HttpContext parameter)
	{
		var result = _previous.Get(parameter);
		if (result)
		{
			var clone = _context.Get(parameter);
			_store.Execute(clone);
		}

		return result;
	}
}
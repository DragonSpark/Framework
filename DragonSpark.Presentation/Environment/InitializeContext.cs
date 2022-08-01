using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class InitializeContext : IInitializeContext
{
	readonly ContextStore             _store;
	readonly IAlteration<HttpContext> _context;

	public InitializeContext(ContextStore store) : this(store, CloneHttpContext.Default) {}

	public InitializeContext(ContextStore store, IAlteration<HttpContext> context)
	{
		_store   = store;
		_context = context;
	}

	public void Execute(HttpContext parameter)
	{
		var clone = _context.Get(parameter);
		_store.Execute(clone);
	}
}
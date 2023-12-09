using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class EstablishContext : IEstablishContext
{
	readonly ContextStore             _store;
	readonly IAlteration<HttpContext> _context;

	public EstablishContext(ContextStore store, IDetermineContext context)
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
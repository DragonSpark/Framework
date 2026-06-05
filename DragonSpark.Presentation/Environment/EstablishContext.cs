using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class EstablishContext : IEstablishContext
{
	readonly ContextStore _store;

	public EstablishContext(ContextStore store) => _store = store;

	public void Execute(HttpContext parameter)
	{
		_store.Execute(parameter);
	}
}
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class Enter : IEnter
{
	readonly IHttpContextAccessor _accessor;
	readonly ContextStore         _store;

	public Enter(IHttpContextAccessor accessor, ContextStore store)
	{
		_accessor = accessor;
		_store    = store;
	}

	public ValueTask Get(CancellationToken parameter)
	{
		_accessor.HttpContext ??= _store.Get();
		return ValueTask.CompletedTask;
	}
}
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

public abstract class EnsureEstablishedContext<T> : IStopAware<T>
{
	readonly IStopAware<T>        _previous;
	readonly ContextStore         _store;
	readonly IHttpContextAccessor _accessor;

	protected EnsureEstablishedContext(IStopAware<T> previous, ContextStore store, IHttpContextAccessor accessor)
	{
		_previous = previous;
		_store    = store;
		_accessor = accessor;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		_accessor.HttpContext ??= _store;
		await _previous.Off(parameter);
	}
}
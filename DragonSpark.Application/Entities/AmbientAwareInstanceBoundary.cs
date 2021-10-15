using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities;

sealed class AmbientAwareInstanceBoundary : IBoundary
{
	readonly IBoundary             _previous;
	readonly IResult<IDisposable?> _current;
	readonly IDisposable           _default;

	public AmbientAwareInstanceBoundary(InstanceBoundary previous)
		: this(previous, AmbientLock.Default, EmptyDisposable.Default) {}

	public AmbientAwareInstanceBoundary(IBoundary previous, IResult<IDisposable?> current, IDisposable @default)
	{
		_previous = previous;
		_current  = current;
		_default  = @default;
	}

	public async ValueTask<IDisposable> Get()
	{
		var disposable = _current.Get();
		var result     = disposable is null ? await _previous.Get() : _default;
		return result;
	}
}
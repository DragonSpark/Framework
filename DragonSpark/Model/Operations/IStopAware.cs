using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public interface IStopAware<T> : IOperation<Stop<T>>;

public interface IStopAware : IOperation<CancellationToken>;

// TODO

public class StopAware<T> : Operation<Stop<T>>, IStopAware<T>
{
	public StopAware(ISelect<Stop<T>, ValueTask> select) : base(select) {}

	public StopAware(Func<Stop<T>, ValueTask> select) : base(select) {}
}

sealed class StopAwareOperationAdapter<T> : IOperation<T>
{
	readonly ISelect<Stop<T>, ValueTask> _previous;

	public StopAwareOperationAdapter(ISelect<Stop<T>, ValueTask> previous) => _previous = previous;

	public ValueTask Get(T parameter) => _previous.Get(new(parameter));
}

sealed class StopAwareAdapter<T> : IStopAware<T>
{
	readonly ISelect<T, ValueTask> _previous;

	public StopAwareAdapter(ISelect<T, ValueTask> previous) => _previous = previous;

	public ValueTask Get(Stop<T> parameter) => _previous.Get(parameter.Subject);
}

public interface IStopAdaptor<T> : IStopAware<T>
{
	IOperation<T> Alternate { get; }
}

public class StopAdaptor<T> : StopAware<T>, IStopAdaptor<T>
{
	protected StopAdaptor(IStopAware<T> stop) : this(stop, new StopAwareOperationAdapter<T>(stop)) {}

	protected StopAdaptor(IStopAware<T> stop, IOperation<T> selecting)
		: base(stop) => Alternate = selecting;

	public IOperation<T> Alternate { get; }
}

sealed class Terminate<TIn, TTo> : IStopAware<TIn>
{
	readonly ISelect<Stop<TIn>, ValueTask<TTo>> _previous;
	readonly Func<Stop<TTo>, ValueTask>         _command;

	public Terminate(ISelect<Stop<TIn>, ValueTask<TTo>> previous, Func<Stop<TTo>, ValueTask> command)
	{
		_previous = previous;
		_command  = command;
	}

	public async ValueTask Get(Stop<TIn> parameter)
	{
		var previous = await _previous.Off(parameter);
		await _command(new(previous, parameter)).Off();
	}
}


public class StopAwareAppending<T> : IStopAware<T>
{
	readonly Await<Stop<T>> _first, _second;

	public StopAwareAppending(IStopAware<T> first, IStopAware<T> second) : this(first.Off, second.Off) {}

	public StopAwareAppending(Await<Stop<T>> first, Await<Stop<T>> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		await _first(parameter);
		await _second(parameter);
	}
}
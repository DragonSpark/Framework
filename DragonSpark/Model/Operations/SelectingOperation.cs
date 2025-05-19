using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class SelectingOperation<T> : IOperation
{
	readonly AwaitOf<T> _previous;
	readonly Await<T>   _select;

	public SelectingOperation(IResulting<T> previous, ISelect<T, ValueTask> select) : this(previous, select.Off) {}

	public SelectingOperation(IResulting<T> previous, Await<T> select) : this(previous.Off, select) {}

	public SelectingOperation(AwaitOf<T> previous, Await<T> select)
	{
		_previous = previous;
		_select   = @select;
	}

	public async ValueTask Get()
	{
		var previous = await _previous();
		await _select(previous);
	}
}

public class StopAwareSelectingOperation<T> : IStopAware
{
	readonly Await<CancellationToken, T> _previous;
	readonly Await<Stop<T>>              _select;

	public StopAwareSelectingOperation(ISelect<CancellationToken, ValueTask<T>> previous,
	                                   ISelect<Stop<T>, ValueTask> select)
		: this(previous, select.Off) {}

	public StopAwareSelectingOperation(ISelect<CancellationToken, ValueTask<T>> previous, Await<Stop<T>> select)
		: this(previous.Off, select) {}

	public StopAwareSelectingOperation(Await<CancellationToken, T> previous, Await<Stop<T>> select)
	{
		_previous = previous;
		_select   = @select;
	}

	public async ValueTask Get(CancellationToken parameter)
	{
		var previous = await _previous(parameter);
		await _select(new(previous, parameter));
	}
}
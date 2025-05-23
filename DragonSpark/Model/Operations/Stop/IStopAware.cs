using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

public interface IStopAware<T> : IOperation<Stop<T>>;

public interface IStopAware : IOperation<CancellationToken>;

// TODO

public class SelectingOperation<T> : IStopAware
{
	readonly Await<CancellationToken, T> _previous;
	readonly Await<Stop<T>>              _select;

	public SelectingOperation(ISelect<CancellationToken, ValueTask<T>> previous,
	                                   ISelect<Stop<T>, ValueTask> select)
		: this(previous, select.Off) {}

	public SelectingOperation(ISelect<CancellationToken, ValueTask<T>> previous, Await<Stop<T>> select)
		: this(previous.Off, select) {}

	public SelectingOperation(Await<CancellationToken, T> previous, Await<Stop<T>> select)
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
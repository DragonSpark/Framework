using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class SelectingOperation<T> : IOperation
{
	readonly AwaitOf<T> _previous;
	readonly Await<T>   _select;

	public SelectingOperation(IResulting<T> previous, ISelect<T, ValueTask> select) : this(previous, select.Await) {}

	public SelectingOperation(IResulting<T> previous, Await<T> select) : this(previous.Await, select) {}

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
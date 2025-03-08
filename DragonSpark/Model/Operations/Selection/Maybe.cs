using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

public class Maybe<TIn, TOut> : ISelecting<TIn, TOut?>
{
	readonly Await<TIn, TOut?> _first, _second;

	public Maybe(ISelecting<TIn, TOut?> first, ISelecting<TIn, TOut?> second) : this(first.Off, second.Off) {}

	public Maybe(Await<TIn, TOut?> first, Await<TIn, TOut?> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<TOut?> Get(TIn parameter) => await _first(parameter) ?? await _second(parameter);
}
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class CoalesceStructure<TIn, TOut> : ISelecting<TIn, TOut> where TOut : struct
{
	readonly Await<TIn, TOut?> _first;
	readonly Await<TIn, TOut>  _second;

	protected CoalesceStructure(ISelecting<TIn, TOut?> first, ISelecting<TIn, TOut> second)
		: this(first.Await, second.Await) {}

	protected CoalesceStructure(Await<TIn, TOut?> first, Await<TIn, TOut> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<TOut> Get(TIn parameter) => await _first(parameter) ?? await _second(parameter);
}
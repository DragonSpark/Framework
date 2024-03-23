using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

[UsedImplicitly]
public class Assuming<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly Func<ISelecting<TIn, TOut>> _previous;

	protected Assuming(Func<ISelecting<TIn, TOut>> previous) => _previous = previous;

	public ValueTask<TOut> Get(TIn parameter) => _previous().Get(parameter);
}
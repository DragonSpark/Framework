using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Assuming : IOperation
	{
		readonly Func<IOperation> _previous;

		public Assuming(Func<IOperation> previous) => this._previous = previous;

		public ValueTask Get() => _previous().Get();
	}

	public class Assuming<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly Func<ISelecting<TIn, TOut>> _previous;

		protected Assuming(Func<ISelecting<TIn, TOut>> previous) => _previous = previous;

		public ValueTask<TOut> Get(TIn parameter) => _previous().Get(parameter);
	}

	public class Assuming<T> : IOperation<T>
	{
		readonly Func<IOperation<T>> _previous;

		public Assuming(Func<IOperation<T>> previous) => _previous = previous;

		public ValueTask Get(T parameter) => _previous().Get(parameter);
	}
}
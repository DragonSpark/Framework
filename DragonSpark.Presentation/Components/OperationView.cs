using DragonSpark.Model.Operations;
using System.Collections.Concurrent;
using System.Threading.Tasks;
// ReSharper disable PossibleMultipleConsumption

namespace DragonSpark.Presentation.Components
{
	public sealed class OperationView<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly ISelecting<TIn, TOut>      _view;
		readonly ConcurrentStack<ValueTask<TOut>> _operations;

		public OperationView(ISelecting<TIn, TOut> view) : this(view, new ConcurrentStack<ValueTask<TOut>>()) {}

		public OperationView(ISelecting<TIn, TOut> view, ConcurrentStack<ValueTask<TOut>> operations)
		{
			_view       = view;
			_operations = operations;
		}

		public bool IsActive => _operations.Count > 0;

		public async ValueTask<TOut> Get(TIn parameter)
		{
			var operation = _view.Get(parameter);
			_operations.Push(operation);
			var result = await operation;

			_operations.TryPop(out _);

			return result;
		}
	}
}
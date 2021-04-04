using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	sealed class OperationResulting<TIn, TOut> : IResulting<TOut>
	{
		readonly Func<ValueTask<TIn>> _previous;
		readonly Func<TIn, TOut>      _select;
		readonly bool                 _capture;

		public OperationResulting(Func<ValueTask<TIn>> previous, Func<TIn, TOut> select, bool capture = false)
		{
			_previous = previous;
			_select   = select;
			_capture  = capture;
		}

		public async ValueTask<TOut> Get()
		{
			var previous = _previous();
			if (previous.IsCompletedSuccessfully)
			{
				return _select(previous.Result);
			}

			var input  = await previous.ConfigureAwait(_capture);
			var result = _select(input);
			return result;
		}
	}
}
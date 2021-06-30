using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	sealed class Binding<T> : IOperation
	{
		readonly IOperation<T>      _operation;
		readonly Func<ValueTask<T>> _parameter;
		readonly bool               _capture;

		public Binding(IOperation<T> operation, Func<ValueTask<T>> parameter, bool capture = false)
		{
			_operation = operation;
			_parameter = parameter;
			_capture   = capture;
		}

		public async ValueTask Get()
		{
			var parameter = _parameter();
			var input = parameter.IsCompletedSuccessfully ? parameter.Result : await parameter.ConfigureAwait(_capture);
			await _operation.Await(input);
		}
	}
}
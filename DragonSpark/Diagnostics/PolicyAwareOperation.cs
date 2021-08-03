using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model.Operations;
using Polly;
using System.Threading.Tasks;

namespace DragonSpark.Diagnostics
{
	public class PolicyAwareOperation<T> : IOperation<T>
	{
		readonly OperationContext<T> _previous;
		readonly IAsyncPolicy        _policy;

		public PolicyAwareOperation(IOperation<T> previous, IAsyncPolicy policy)
			: this(previous.Then(), policy) {}

		public PolicyAwareOperation(OperationContext<T> previous, IAsyncPolicy policy)
		{
			_previous = previous;
			_policy   = policy;
		}

		public ValueTask Get(T parameter)
			=> _policy.ExecuteAsync(_previous.Bind(parameter).Select(x => x.AsTask()).Get().Get).ToOperation();
	}
}
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model.Operations;
using Polly;
using System.Threading.Tasks;

namespace DragonSpark.Diagnostics
{
	public class PolicyAwareSelecting<TIn, TResult> : ISelecting<TIn, TResult>
	{
		readonly OperationResultSelector<TIn, TResult> _previous;
		readonly IAsyncPolicy                          _policy;

		public PolicyAwareSelecting(ISelecting<TIn, TResult> previous, IAsyncPolicy policy)
			: this(previous.Then(), policy) {}

		public PolicyAwareSelecting(OperationResultSelector<TIn, TResult> previous, IAsyncPolicy policy)
		{
			_previous = previous;
			_policy   = policy;
		}

		public ValueTask<TResult> Get(TIn parameter)
			=> _policy.ExecuteAsync(_previous.Bind(parameter).Select(x => x.AsTask()).Get().Get).ToOperation();
	}
}
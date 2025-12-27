using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using Polly;
using System.Threading.Tasks;

namespace DragonSpark.Diagnostics;

public class PolicyAwareSelecting<TIn, TOut> : ISelecting<TIn, TOut>  // TODO: StopAware
{
	readonly OperationResultComposer<TIn, TOut> _previous;
	readonly IAsyncPolicy<TOut>                 _policy;

	protected PolicyAwareSelecting(ISelecting<TIn, TOut> previous, IAsyncPolicy policy)
		: this(previous.Then(), policy) {}

	protected PolicyAwareSelecting(OperationResultComposer<TIn, TOut> previous, IAsyncPolicy policy)
		: this(previous, policy.AsAsyncPolicy<TOut>()) {}

	protected PolicyAwareSelecting(ISelecting<TIn, TOut> previous, IAsyncPolicy<TOut> policy)
		: this(previous.Then(), policy) {}

	protected PolicyAwareSelecting(OperationResultComposer<TIn, TOut> previous, IAsyncPolicy<TOut> policy)
	{
		_previous = previous;
		_policy   = policy;
	}

	public ValueTask<TOut> Get(TIn parameter)
		=> _policy.ExecuteAsync(_previous.Bind(parameter).Select(x => x.AsTask()).Get().Get).ToOperation();
}
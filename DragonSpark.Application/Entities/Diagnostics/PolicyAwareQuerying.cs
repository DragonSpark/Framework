using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using Polly;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Diagnostics
{
	sealed class PolicyAwareQuerying<T, TResult> : IQuerying<T, TResult>

	{
		readonly IQuerying<T, TResult> _previous;
		readonly IAsyncPolicy          _policy;

		public PolicyAwareQuerying(IQuerying<T, TResult> previous, IAsyncPolicy policy)
		{
			_previous = previous;
			_policy   = policy;
		}

		public ValueTask<TResult> Get(IQueryable<T> parameter)
			=> _policy.ExecuteAsync(_previous.Get(parameter).AsTask).ToOperation();
	}
}
using DragonSpark.Compose;
using Polly;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class PolicyAwareActiveContent<T> : IActiveContent<T>
	{
		readonly IActiveContent<T> _previous;
		readonly Func<Task<T>>     _operation;
		readonly IAsyncPolicy      _policy;

		public PolicyAwareActiveContent(IActiveContent<T> previous, IAsyncPolicy policy)
			: this(previous, previous.Then().Allocate(), policy) {}

		public PolicyAwareActiveContent(IActiveContent<T> previous, Func<Task<T>> operation, IAsyncPolicy policy)
		{
			_previous  = previous;
			_operation = operation;
			_policy    = policy;
		}

		public ValueTask<T> Get() => _policy.ExecuteAsync(_operation).ToOperation();

		public bool HasValue => _previous.HasValue;
	}
}
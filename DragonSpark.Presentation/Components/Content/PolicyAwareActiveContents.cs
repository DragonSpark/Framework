using Polly;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	public class PolicyAwareActiveContents<T> : IActiveContents<T>
	{
		readonly IActiveContents<T> _previous;
		readonly IAsyncPolicy       _policy;

		public PolicyAwareActiveContents(IAsyncPolicy policy) : this(ActiveContents<T>.Default, policy) {}

		public PolicyAwareActiveContents(IActiveContents<T> previous, IAsyncPolicy policy)
		{
			_previous = previous;
			_policy   = policy;
		}

		public IActiveContent<T> Get(Func<ValueTask<T>> parameter)
			=> new PolicyAwareActiveContent<T>(_previous.Get(parameter), _policy);
	}
}
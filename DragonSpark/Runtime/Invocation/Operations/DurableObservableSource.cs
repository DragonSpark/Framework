using Polly;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Runtime.Invocation.Operations
{
	public class DurableObservableSource<T> : IObserve<T>
	{
		readonly Func<Func<T>, T> _policy;

		public DurableObservableSource(ISyncPolicy policy) : this(policy.Execute) {}

		public DurableObservableSource(ISyncPolicy<T> policy) : this(policy.Execute) {}

		public DurableObservableSource(Func<Func<T>, T> policy) => _policy = policy;

		public T Get(Task<T> parameter) => _policy(parameter.Wait);
	}
}
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Polly;
using System;

namespace DragonSpark.Diagnostics
{
	class Class1 {}

	public sealed class DefaultCircuitBreakerPolicy<T> : CircuitBreakerPolicy<T>
	{
		public static DefaultCircuitBreakerPolicy<T> Default { get; } = new();

		DefaultCircuitBreakerPolicy() {}
	}

	public interface IPolicy<T> : ISelect<PolicyBuilder<T>, IAsyncPolicy<T>> {}

	public class CircuitBreakerPolicy<T> : IPolicy<T>
	{
		readonly byte     _attempts;
		readonly TimeSpan _break;

		protected CircuitBreakerPolicy() : this(5, TimeSpan.FromSeconds(30)) {}

		public CircuitBreakerPolicy(byte attempts, TimeSpan @break)
		{
			_attempts = attempts;
			_break    = @break;
		}

		public IAsyncPolicy<T> Get(PolicyBuilder<T> parameter) => parameter.CircuitBreakerAsync(_attempts, _break);
	}

	public sealed class DefaultRetryPolicy<T> : RetryPolicy<T>
	{
		public static DefaultRetryPolicy<T> Default { get; } = new DefaultRetryPolicy<T>();

		DefaultRetryPolicy() {}
	}

	public class RetryPolicy<T> : IPolicy<T>
	{
		readonly byte                _times;
		readonly Func<int, TimeSpan> _strategy;

		protected RetryPolicy() : this(5, JitterStrategy.Default.Get) {}

		public RetryPolicy(byte times, Func<int, TimeSpan> strategy)
		{
			_times    = times;
			_strategy = strategy;
		}

		public IAsyncPolicy<T> Get(PolicyBuilder<T> parameter) => parameter.WaitAndRetryAsync(_times, _strategy);
	}

	public class Policy<T> : Instance<PolicyBuilder<T>>
	{
		public Policy(PolicyBuilder<T> instance) : base(instance) {}
	}

/**/

	public sealed class DefaultCircuitBreakerPolicy : CircuitBreakerPolicy
	{
		public static DefaultCircuitBreakerPolicy Default { get; } = new();

		DefaultCircuitBreakerPolicy() {}
	}

	public interface IPolicy : ISelect<PolicyBuilder, IAsyncPolicy> {}

	public class CircuitBreakerPolicy : IPolicy
	{
		readonly byte     _attempts;
		readonly TimeSpan _break;

		protected CircuitBreakerPolicy() : this(5, TimeSpan.FromSeconds(30)) {}

		public CircuitBreakerPolicy(byte attempts, TimeSpan @break)
		{
			_attempts = attempts;
			_break    = @break;
		}

		public IAsyncPolicy Get(PolicyBuilder parameter) => parameter.CircuitBreakerAsync(_attempts, _break);
	}

	public sealed class DefaultRetryPolicy : RetryPolicy
	{
		public static DefaultRetryPolicy Default { get; } = new DefaultRetryPolicy();

		DefaultRetryPolicy() {}
	}

	public class RetryPolicy : IPolicy
	{
		readonly byte                _times;
		readonly Func<int, TimeSpan> _strategy;

		protected RetryPolicy() : this(5, JitterStrategy.Default.Get) {}

		public RetryPolicy(byte times, Func<int, TimeSpan> strategy)
		{
			_times    = times;
			_strategy = strategy;
		}

		public IAsyncPolicy Get(PolicyBuilder parameter) => parameter.WaitAndRetryAsync(_times, _strategy);
	}

	public class Policy : Instance<PolicyBuilder>
	{
		public Policy(PolicyBuilder instance) : base(instance) {}
	}
}
using DragonSpark.Aspects.Exceptions;
using DragonSpark.Sources.Parameterized;
using Polly;
using System;

namespace DragonSpark.Diagnostics.Exceptions
{
	public sealed class RetryPolicySource<T> : RetryPolicySource where T : Exception
	{
		public static RetryPolicySource<T> Default { get; } = new RetryPolicySource<T>();
		RetryPolicySource() : this( Time.Default ) {}

		public RetryPolicySource( Func<int, TimeSpan> time ) : base( PolicyBuilderSource<T>.Default.Get, time ) {}
	}

	public class RetryPolicySource : ParameterizedSourceBase<int, Policy>
	{
		readonly static Func<Action<Exception, TimeSpan>> OnRetry = RetryDelegateFactory.Default.Get;

		readonly Func<PolicyBuilder> source;
		readonly Func<int, TimeSpan> time;
		readonly Func<Action<Exception, TimeSpan>> onRetry;

		public RetryPolicySource( Func<PolicyBuilder> source ) : this( source, Time.Default ) {}

		public RetryPolicySource( Func<PolicyBuilder> source, Func<int, TimeSpan> time ) : this( source, time, OnRetry ) {}

		public RetryPolicySource( Func<PolicyBuilder> source, Func<int, TimeSpan> time, Func<Action<Exception, TimeSpan>> onRetry )
		{
			this.source = source;
			this.time = time;
			this.onRetry = onRetry;
		}

		public override Policy Get( int parameter ) => source().WaitAndRetry( parameter, time, onRetry() );
	}
}
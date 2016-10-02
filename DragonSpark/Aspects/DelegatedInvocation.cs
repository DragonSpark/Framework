using System;

namespace DragonSpark.Aspects
{
	sealed class DelegatedInvocation<TParameter, TResult> : IInvocation
	{
		readonly Func<TParameter, TResult> source;
		public DelegatedInvocation( Func<TParameter, TResult> source )
		{
			this.source = source;
		}

		public object Invoke( object parameter ) => source( (TParameter)parameter );
	}
}
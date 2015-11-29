using System;
using System.Windows.Threading;

namespace DragonSpark.Application.Client.Threading
{
	public static class DispatcherExtensions
	{
		public static TResult Resolve<TResult>( this Dispatcher @this, Func<TResult> resolver )
		{
			var operation = @this.BeginInvoke( new Func<TResult>( resolver ) );
			operation.Task.Wait();
			var result = (TResult)operation.Result;
			return result;
		}
	}
}
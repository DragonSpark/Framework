using System;

namespace DragonSpark.Diagnostics.Exceptions
{
	public static class Retry
	{
		public static void Execute<T>( Action action ) where T : Exception => SuppliedRetryPolicySource<T>.Default.Get().Execute( action );
	}
}
using System;

namespace DragonSpark.Diagnostics.Exceptions
{
	public sealed class BackOffRetryTime : RetryTimeBase
	{
		public static BackOffRetryTime Default { get; } = new BackOffRetryTime();
		BackOffRetryTime() : base( parameter => (int)Math.Pow( parameter, 2 ) ) {}
	}
}
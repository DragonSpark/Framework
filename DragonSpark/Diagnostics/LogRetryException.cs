using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using Serilog;
using System;

namespace DragonSpark.Diagnostics
{
	public sealed class LogRetryException : LogExceptionCommandBase<TimeSpan>
	{
		public static IParameterizedSource<ILogger, LogRetryException> Defaults { get; } = new Cache<ILogger, LogRetryException>( logger => new LogRetryException( logger ) );
		LogRetryException( ILogger logger ) : this( logger.Information ) {}

		public LogRetryException( LogException<TimeSpan> action ) : base( action, "Exception encountered during a retry-aware context.  Waiting {Wait} until next attempt..." ) {}
	}
}
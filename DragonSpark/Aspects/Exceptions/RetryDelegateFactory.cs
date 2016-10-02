using DragonSpark.Diagnostics;
using DragonSpark.Sources;
using Serilog;
using System;

namespace DragonSpark.Aspects.Exceptions
{
	class RetryDelegateFactory : SourceBase<Action<Exception, TimeSpan>>
	{
		public static RetryDelegateFactory Default { get; } = new RetryDelegateFactory();
		RetryDelegateFactory() : this( Diagnostics.Defaults.Factory, LogRetryException.Defaults.Get ) {}

		readonly Func<ILogger> loggerSource;
		readonly Func<ILogger, LogExceptionCommandBase<TimeSpan>> commandSource;

		public RetryDelegateFactory( Func<ILogger> loggerSource, Func<ILogger, LogExceptionCommandBase<TimeSpan>> commandSource )
		{
			this.loggerSource = loggerSource;
			this.commandSource = commandSource;
		}

		public override Action<Exception, TimeSpan> Get() => commandSource( loggerSource() ).Execute;
	}
}
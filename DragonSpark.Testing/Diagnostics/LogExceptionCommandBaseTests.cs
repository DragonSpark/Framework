using DragonSpark.Diagnostics;
using JetBrains.Annotations;
using Serilog;
using System;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class LogExceptionCommandBaseTests
	{
		[Theory, Framework.Application.AutoData]
		void Log( LogExceptionCommand command, Exception error ) => command.Execute( error );

		[Theory, Framework.Application.AutoData]
		void LogListOne( LogExceptionCommand<string> command, Exception error, string message ) => command.Execute( error, message );

		[Theory, Framework.Application.AutoData]
		void LogListTwo( LogExceptionCommand<string, DateTime> command, Exception error, string message, DateTime dateTime ) => command.Execute( error, message, dateTime );

		[Theory, Framework.Application.AutoData]
		void LogListThree( LogExceptionCommand<string, DateTime, int> command, Exception error, string message, DateTime dateTime, int number ) => command.Execute( error, message, dateTime, number );

		[UsedImplicitly]
		sealed class LogExceptionCommand : LogExceptionCommandBase
		{
			[UsedImplicitly]
			public LogExceptionCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}
			[UsedImplicitly]
			public LogExceptionCommand( LogException action, string messageTemplate ) : base( action, messageTemplate ) {}
		}

		[UsedImplicitly]
		sealed class LogExceptionCommand<T> : LogExceptionCommandBase<T>
		{
			[UsedImplicitly]
			public LogExceptionCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}
			[UsedImplicitly]
			public LogExceptionCommand( LogException<T> action, string messageTemplate ) : base( action, messageTemplate ) {}
		}

		[UsedImplicitly]
		sealed class LogExceptionCommand<T1, T2> : LogExceptionCommandBase<T1, T2>
		{
			[UsedImplicitly]
			public LogExceptionCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}
			[UsedImplicitly]
			public LogExceptionCommand( LogException<T1, T2> action, string messageTemplate ) : base( action, messageTemplate ) {}
		}

		[UsedImplicitly]
		sealed class LogExceptionCommand<T1, T2, T3> : LogExceptionCommandBase<T1, T2, T3>
		{
			[UsedImplicitly]
			public LogExceptionCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}
			[UsedImplicitly]
			public LogExceptionCommand( LogException<T1, T2, T3> action, string messageTemplate ) : base( action, messageTemplate ) {}
		}
	}
}
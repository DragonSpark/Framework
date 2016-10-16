using DragonSpark.Diagnostics;
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
		void Log( LogExceptionCommand<string> command, Exception error, string message ) => command.Execute( error, message );

		[Theory, Framework.Application.AutoData]
		void Log( LogExceptionCommand<string, DateTime> command, Exception error, string message, DateTime dateTime ) => command.Execute( error, message, dateTime );

		[Theory, Framework.Application.AutoData]
		void Log( LogExceptionCommand<string, DateTime, int> command, Exception error, string message, DateTime dateTime, int number ) => command.Execute( error, message, dateTime, number );

		sealed class LogExceptionCommand : LogExceptionCommandBase
		{
			public LogExceptionCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}
			public LogExceptionCommand( LogException action, string messageTemplate ) : base( action, messageTemplate ) {}
		}

		sealed class LogExceptionCommand<T> : LogExceptionCommandBase<T>
		{
			public LogExceptionCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}
			public LogExceptionCommand( LogException<T> action, string messageTemplate ) : base( action, messageTemplate ) {}
		}

		sealed class LogExceptionCommand<T1, T2> : LogExceptionCommandBase<T1, T2>
		{
			public LogExceptionCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}
			public LogExceptionCommand( LogException<T1, T2> action, string messageTemplate ) : base( action, messageTemplate ) {}
		}

		sealed class LogExceptionCommand<T1, T2, T3> : LogExceptionCommandBase<T1, T2, T3>
		{
			public LogExceptionCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}
			public LogExceptionCommand( LogException<T1, T2, T3> action, string messageTemplate ) : base( action, messageTemplate ) {}
		}
	}
}
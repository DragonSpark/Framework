using DragonSpark.Diagnostics;
using DragonSpark.TypeSystem;
using Serilog;
using System;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class LogCommandBaseTests
	{
		[Theory, Framework.Application.AutoData]
		void Log( LogCommand command )
		{
			command.Execute( Items<object>.Default );
			command.ExecuteUsing( new object() );
		}

		[Theory, Framework.Application.AutoData]
		void Log( LogCommand<string, DateTime, int> command, string message, DateTime dateTime, int number ) => command.Execute( message, dateTime, number );

		sealed class LogCommand<T1, T2, T3> : LogCommandBase<T1, T2, T3>
		{
			public LogCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}
			public LogCommand( LogTemplate<T1, T2, T3> action, string messageTemplate ) : base( action, messageTemplate ) {}
		}

		sealed class LogCommand : LogCommandBase
		{
			public LogCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}
			public LogCommand( LogTemplate action, string messageTemplate ) : base( action, messageTemplate ) {}
		}
	}
}
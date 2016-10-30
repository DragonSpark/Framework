using DragonSpark.Diagnostics;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
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
		void LogList( LogCommand<string, DateTime, int> command, string message, DateTime dateTime, int number ) => command.Execute( message, dateTime, number );

		[UsedImplicitly]
		sealed class LogCommand<T1, T2, T3> : LogCommandBase<T1, T2, T3>
		{
			[UsedImplicitly]
			public LogCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}

			[UsedImplicitly]
			public LogCommand( LogTemplate<T1, T2, T3> action, string messageTemplate ) : base( action, messageTemplate ) {}
		}

		[UsedImplicitly]
		sealed class LogCommand : LogCommandBase
		{
			[UsedImplicitly]
			public LogCommand( ILogger logger, string messageTemplate ) : base( logger, messageTemplate ) {}

			[UsedImplicitly]
			public LogCommand( LogTemplate action, string messageTemplate ) : base( action, messageTemplate ) {}
		}
	}
}
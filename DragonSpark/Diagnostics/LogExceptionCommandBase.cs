using DragonSpark.Commands;
using Serilog;
using System;

namespace DragonSpark.Diagnostics
{
	public abstract class LogExceptionCommandBase<T> : CommandBase<ExceptionParameter<T>>
	{
		readonly LogException<T> action;
		readonly string messageTemplate;

		protected LogExceptionCommandBase( ILogger logger, string messageTemplate ) : this( logger.Information, messageTemplate ) {}
		protected LogExceptionCommandBase( LogException<T> action, string messageTemplate )
		{
			this.action = action;
			this.messageTemplate = messageTemplate;
		}

		public override void Execute( ExceptionParameter<T> parameter ) => action( parameter.Exception, messageTemplate, parameter.Argument );

		public void Execute( Exception exception, T argument ) => Execute( new ExceptionParameter<T>( exception, argument ) );
	}
}
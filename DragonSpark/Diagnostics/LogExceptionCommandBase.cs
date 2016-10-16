using DragonSpark.Commands;
using DragonSpark.Runtime;
using Serilog;
using System;

namespace DragonSpark.Diagnostics
{
	public abstract class LogExceptionCommandBase : CommandBase<ExceptionParameter<object[]>>
	{
		readonly LogException action;
		readonly string messageTemplate;

		protected LogExceptionCommandBase( ILogger logger, string messageTemplate ) : this( logger.Information, messageTemplate ) {}
		protected LogExceptionCommandBase( LogException action, string messageTemplate )
		{
			this.action = action;
			this.messageTemplate = messageTemplate;
		}

		public override void Execute( ExceptionParameter<object[]> parameter ) => action( parameter.Exception, messageTemplate, parameter.Argument );

		public void Execute( Exception exception, params object[] arguments ) => Execute( new ExceptionParameter<object[]>( exception, arguments ) );
	}

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

	public abstract class LogExceptionCommandBase<T1, T2> : CommandBase<ExceptionParameter<ValueTuple<T1, T2>>>
	{
		readonly LogException<T1, T2> action;
		readonly string messageTemplate;

		protected LogExceptionCommandBase( ILogger logger, string messageTemplate ) : this( logger.Information, messageTemplate ) {}
		protected LogExceptionCommandBase( LogException<T1, T2> action, string messageTemplate )
		{
			this.action = action;
			this.messageTemplate = messageTemplate;
		}

		public override void Execute( ExceptionParameter<ValueTuple<T1, T2>> parameter ) => action( parameter.Exception, messageTemplate, parameter.Argument.Item1, parameter.Argument.Item2 );

		public void Execute( Exception exception, T1 first, T2 second ) => Execute( new ExceptionParameter<ValueTuple<T1, T2>>( exception, new ValueTuple<T1,T2>( first, second ) ) );
	}

	public abstract class LogExceptionCommandBase<T1, T2, T3> : CommandBase<ExceptionParameter<ValueTuple<T1, T2, T3>>>
	{
		readonly LogException<T1, T2, T3> action;
		readonly string messageTemplate;

		protected LogExceptionCommandBase( ILogger logger, string messageTemplate ) : this( logger.Information, messageTemplate ) {}
		protected LogExceptionCommandBase( LogException<T1, T2, T3> action, string messageTemplate )
		{
			this.action = action;
			this.messageTemplate = messageTemplate;
		}

		public override void Execute( ExceptionParameter<ValueTuple<T1, T2, T3>> parameter ) => action( parameter.Exception, messageTemplate, parameter.Argument.Item1, parameter.Argument.Item2, parameter.Argument.Item3 );

		public void Execute( Exception exception, T1 first, T2 second, T3 third ) => Execute( new ExceptionParameter<ValueTuple<T1, T2, T3>>( exception, new ValueTuple<T1,T2, T3>( first, second, third ) ) );
	}
}
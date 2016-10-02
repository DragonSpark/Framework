using DragonSpark.Commands;
using DragonSpark.Runtime;
using Serilog;
using System;

namespace DragonSpark.Diagnostics
{
	public delegate void LogTemplate<in T>( string template, T parameter );
	public delegate void LogTemplate<in T1, in T2>( string template, T1 first, T2 second );
	public delegate void LogTemplate<in T1, in T2, in T3>( string template, T1 first, T2 second, T3 third );
	public delegate void LogTemplate( string template, params object[] parameters );

	public abstract class LogCommandBase<T1, T2> : CommandBase<ValueTuple<T1, T2>>
	{
		readonly LogTemplate<T1, T2> action;
		readonly string messageTemplate;

		protected LogCommandBase( ILogger logger, string messageTemplate ) : this( logger.Information, messageTemplate ) {}

		protected LogCommandBase( LogTemplate<T1, T2> action, string messageTemplate )
		{
			this.action = action;
			this.messageTemplate = messageTemplate;
		}

		public override void Execute( ValueTuple<T1, T2> parameter ) => action( messageTemplate, parameter.Item1, parameter.Item2 );

		public void Execute( T1 first, T2 second ) => Execute( new ValueTuple<T1, T2>( first, second ) );
	}

	public abstract class LogCommandBase<T1, T2, T3> : CommandBase<ValueTuple<T1, T2, T3>>
	{
		readonly LogTemplate<T1, T2, T3> action;
		readonly string messageTemplate;

		protected LogCommandBase( ILogger logger, string messageTemplate ) : this( logger.Information, messageTemplate ) {}
		protected LogCommandBase( LogTemplate<T1, T2, T3> action, string messageTemplate )
		{
			this.action = action;
			this.messageTemplate = messageTemplate;
		}

		public override void Execute( ValueTuple<T1, T2, T3> parameter ) => action( messageTemplate, parameter.Item1, parameter.Item2, parameter.Item3 );

		public void Execute( T1 first, T2 second, T3 third ) => Execute( new ValueTuple<T1, T2, T3>( first, second, third ) );
	}

	public abstract class LogCommandBase : CommandBase<object[]>
	{
		readonly LogTemplate action;
		readonly string messageTemplate;

		protected LogCommandBase( ILogger logger, string messageTemplate ) : this( logger.Information, messageTemplate ) {}
		protected LogCommandBase( LogTemplate action, string messageTemplate )
		{
			this.action = action;
			this.messageTemplate = messageTemplate;
		}

		public override void Execute( object[] parameter ) => action( messageTemplate, parameter );

		public void ExecuteUsing( params object[] arguments ) => Execute( arguments );
	}

	public delegate void LogException<in T1, in T2>( Exception exception, string template, T1 first, T2 second );
	public delegate void LogException<in T1, in T2, in T3>( Exception exception, string template, T1 first, T2 second, T3 third );
	public delegate void LogException( Exception exception, string template, params object[] parameters );

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
}
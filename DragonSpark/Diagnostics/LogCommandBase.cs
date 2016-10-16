using DragonSpark.Commands;
using DragonSpark.Runtime;
using Serilog;

namespace DragonSpark.Diagnostics
{
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

	public abstract class LogCommandBase<T> : CommandBase<T>
	{
		readonly LogTemplate<T> action;
		readonly string messageTemplate;

		protected LogCommandBase( ILogger logger, string messageTemplate ) : this( logger.Information, messageTemplate ) {}

		protected LogCommandBase( LogTemplate<T> action, string messageTemplate )
		{
			this.action = action;
			this.messageTemplate = messageTemplate;
		}

		public override void Execute( T parameter ) => action( messageTemplate, parameter );
	}

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
}
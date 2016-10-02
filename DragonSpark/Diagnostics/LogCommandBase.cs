using DragonSpark.Commands;
using Serilog;

namespace DragonSpark.Diagnostics
{
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
}
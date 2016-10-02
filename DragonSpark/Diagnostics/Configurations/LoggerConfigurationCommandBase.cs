using DragonSpark.Commands;
using Serilog;
using System;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class LoggerConfigurationCommandBase<T> : CommandBase<LoggerConfiguration>
	{
		readonly Func<LoggerConfiguration, T> projection;

		protected LoggerConfigurationCommandBase( Func<LoggerConfiguration, T> projection )
		{
			this.projection = projection;
		}

		public sealed override void Execute( LoggerConfiguration parameter ) => Configure( projection( parameter ) );

		protected abstract void Configure( T configuration );
	}
}
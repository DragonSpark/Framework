using DragonSpark.Commands;
using System;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class ConfigureLoggerBase<T> : CommandBase<Serilog.LoggerConfiguration>
	{
		readonly Func<Serilog.LoggerConfiguration, T> projection;

		protected ConfigureLoggerBase( Func<Serilog.LoggerConfiguration, T> projection )
		{
			this.projection = projection;
		}

		public sealed override void Execute( Serilog.LoggerConfiguration parameter ) => Configure( projection( parameter ) );

		protected abstract void Configure( T configuration );
	}
}
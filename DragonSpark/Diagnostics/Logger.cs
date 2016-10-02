using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Configuration;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using Serilog;

namespace DragonSpark.Diagnostics
{
	public sealed class Logger : ConfiguringFactory<object, ILogger>
	{
		public static IParameterizedSource<object, ILogger> Default { get; } = new Logger().ToCache();
		Logger() : base( LoggingConfiguration.Default.ToCache().ToSourceDelegate(), Command.DefaultNested.ToDelegate() ) {}
		
		[ApplyAutoValidation, ApplySpecification( typeof(OncePerScopeSpecification<ILogger>) )]
		sealed class Command : CommandBase<ILogger>
		{
			public static Command DefaultNested { get; } = new Command();
			Command() {}

			public override void Execute( ILogger parameter )
			{
				SystemLogger.Default.Assign( Defaults.Factory );
				PurgeLoggerHistoryCommand.Default.Execute( parameter.Write );
			}
		}
	}

	public sealed class LoggingConfiguration : LoggerBase
	{
		public static IConfigurableFactory<LoggerConfiguration, ILogger> Default { get; } = new LoggingConfiguration();
		LoggingConfiguration() {}
	}
}
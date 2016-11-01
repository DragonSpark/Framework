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
		Logger() : base( LoggingConfiguration.Default.ToCache().ToDelegate(), Command.Implementation.ToDelegate() ) {}
		
		[ApplyAutoValidation, ApplySpecification( typeof(OncePerScopeSpecification<ILogger>) )]
		sealed class Command : CommandBase<ILogger>
		{
			public static Command Implementation { get; } = new Command();
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
		public static IConfigurationProvisionedFactory<LoggerConfiguration, ILogger> Default { get; } = new LoggingConfiguration();
		LoggingConfiguration() {}
	}
}
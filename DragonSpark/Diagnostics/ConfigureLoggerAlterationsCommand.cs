using DragonSpark.Runtime.Assignments;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using Serilog;
using System.Collections.Immutable;

namespace DragonSpark.Diagnostics
{
	/*public sealed class DefaultConfigureLoggingCommand : SuppliedCommand<Func<Func<object, ImmutableArray<IAlteration<LoggerConfiguration>>>>>
	{
		public DefaultConfigureLoggingCommand() : base( ConfigureLoggingCommand.Default, LoggingConfigurationSource.Default.ToDelegate() ) {}
	}*/

	public sealed class ConfigureLoggerAlterationsCommand : AssignGlobalScopeCommand<ImmutableArray<IAlteration<LoggerConfiguration>>>
	{
		public static ConfigureLoggerAlterationsCommand Default { get; } = new ConfigureLoggerAlterationsCommand();
		ConfigureLoggerAlterationsCommand() : base( LoggerAlterations.Default ) {}
	}

	public sealed class LoggerAlterations : SingletonScope<ImmutableArray<IAlteration<LoggerConfiguration>>>
	{
		public static LoggerAlterations Default { get; } = new LoggerAlterations();
		LoggerAlterations() : base( () => DefaultLoggerAlterations.Default.IncludeExports().ToImmutableArray() ) {}
	}
}
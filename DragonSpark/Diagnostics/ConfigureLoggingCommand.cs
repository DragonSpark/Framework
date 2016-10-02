using DragonSpark.Commands;
using DragonSpark.Runtime.Assignments;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using Serilog;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Diagnostics
{
	public sealed class DefaultConfigureLoggingCommand : SuppliedCommand<Func<Func<object, ImmutableArray<IAlteration<LoggerConfiguration>>>>>
	{
		public DefaultConfigureLoggingCommand() : base( ConfigureLoggingCommand.Default, LoggingConfigurationSource.Default.ToDelegate() ) {}
	}

	public sealed class ConfigureLoggingCommand : AssignCommand<Func<Func<object, ImmutableArray<IAlteration<LoggerConfiguration>>>>>
	{
		public static ConfigureLoggingCommand Default { get; } = new ConfigureLoggingCommand();
		ConfigureLoggingCommand() : base( LoggingConfiguration.Default.Configurators ) {}
	}
}
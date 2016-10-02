using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using Serilog;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Diagnostics
{
	public sealed class LoggingConfigurationSource : SuppliedSource<Func<Func<object, ImmutableArray<IAlteration<LoggerConfiguration>>>>>
	{
		public static LoggingConfigurationSource Default { get; } = new LoggingConfigurationSource();
		LoggingConfigurationSource() : this( LoggerExportedConfigurations.Default ) {}

		public LoggingConfigurationSource( IItemSource<IAlteration<LoggerConfiguration>> items ) 
			: base( new DelegatedSource<ImmutableArray<IAlteration<LoggerConfiguration>>>( items.Get ).ToDelegate().Wrap().Self ) {}
	}
}
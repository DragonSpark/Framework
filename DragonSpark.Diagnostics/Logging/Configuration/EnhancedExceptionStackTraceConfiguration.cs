﻿using Serilog;
using Serilog.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	sealed class EnhancedExceptionStackTraceConfiguration : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                                        ILoggingEnrichmentConfiguration
	{
		public static EnhancedExceptionStackTraceConfiguration Default { get; }
			= new EnhancedExceptionStackTraceConfiguration();

		EnhancedExceptionStackTraceConfiguration() : base(x => x.WithDemystifiedStackTraces()) {}
	}
}
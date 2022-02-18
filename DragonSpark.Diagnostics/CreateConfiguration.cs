using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Enrichers.Correlation;

namespace DragonSpark.Diagnostics;

sealed class CreateConfiguration : ISelect<IConfiguration, LoggerConfiguration>
{
	public static CreateConfiguration Default { get; } = new();

	CreateConfiguration() {}

	public LoggerConfiguration Get(IConfiguration parameter)
	{
		var instance = new LoggerConfiguration().Enrich.With(PrimaryAssemblyEnricher.Default,
		                                                     AssemblyDeployInformationEnricher.Default)
		                                        .Enrich.WithClientIp()
		                                        .Enrich.WithClientAgent()
		                                        .Enrich.WithDemystifiedStackTraces()
		                                        .Enrich.WithExceptionStackTraceHash()
		                                        .Enrich.WithEnvironmentName()
		                                        .Enrich.WithEnvironmentUserName()
		                                        .Enrich.FromLogContext()
		                                        .Enrich.WithProcessId()
		                                        .Enrich.WithProcessName()
		                                        .Enrich.WithMemoryUsage()
		                                        .Enrich.WithThreadId()
		                                        .Enrich.WithThreadName()
		                                        .Enrich.WithCorrelationId()
		                                        .Enrich.WithEnvironmentUserName()
		                                        .Enrich;
		var result = EnvironmentLoggerConfigurationExtensions.WithMachineName(instance)
		                                                     .ReadFrom.Configuration(parameter);
		return result;
	}
}
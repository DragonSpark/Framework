using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System;

namespace DragonSpark.Diagnostics.OpenTelemetry;

public class OpenTelemetryConfiguration : ICommand<IServiceCollection>
{
	readonly Action<MeterProviderBuilder>  _metrics;
	readonly Action<TracerProviderBuilder> _tracing;

	protected OpenTelemetryConfiguration(Action<MeterProviderBuilder> metrics, Action<TracerProviderBuilder> tracing)
	{
		_metrics = metrics;
		_tracing = tracing;
	}

	public void Execute(IServiceCollection parameter)
	{
		var builder       = parameter.AddOpenTelemetry().WithMetrics(_metrics).WithTracing(_tracing);
		var configuration = parameter.Configuration();
		var setting       = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
		if (!setting.IsNullOrWhiteSpace())
		{
			builder.UseOtlpExporter();
		}
		// Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
		//if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
		//{
		//    builder.Services.AddOpenTelemetry()
		//       .UseAzureMonitor();
		//}
	}
}
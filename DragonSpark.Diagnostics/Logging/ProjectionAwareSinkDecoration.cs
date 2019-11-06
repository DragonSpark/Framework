using Serilog.Core;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Reflection;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Diagnostics.Logging
{
	public sealed class ProjectionAwareSinkDecoration : LoggerSinkDecoration,
	                                                    IActivateUsing<ILoggingSinkConfiguration>,
	                                                    IActivateUsing<ILogEventSink>
	{
		public ProjectionAwareSinkDecoration(ILogEventSink sink) : this(new SinkConfiguration(sink)) {}

		public ProjectionAwareSinkDecoration(ILoggingSinkConfiguration configuration)
			: base(I<ProjectionAwareSink>.Default.From, configuration) {}
	}
}
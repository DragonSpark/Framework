using DragonSpark.Model.Selection;
using Serilog;
using SerilogTracing;

namespace DragonSpark.Diagnostics;

public class Trace<T> : ISelect<T, Tracing>
{
	readonly ActivityListenerConfiguration _configuration;
	readonly ILogger                       _logger;
	readonly string                        _template;

	protected Trace(ActivityListenerConfiguration configuration, ILogger logger, string template)
	{
		_configuration = configuration;
		_logger        = logger;
		_template      = template;
	}

	public Tracing Get(T parameter)
		// ReSharper disable once TemplateIsNotCompileTimeConstantProblem
		=> new(_configuration.TraceTo(_logger), _logger.StartActivity(_template, parameter));
}
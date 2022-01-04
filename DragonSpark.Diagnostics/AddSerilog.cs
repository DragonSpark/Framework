using DragonSpark.Model.Commands;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class AddSerilog : ICommand<ILoggingBuilder>
{
	readonly ILogger _logger;

	public AddSerilog(ILogger logger) => _logger = logger;

	public void Execute(ILoggingBuilder parameter)
	{
		parameter.AddSerilog(_logger, true);
	}
}
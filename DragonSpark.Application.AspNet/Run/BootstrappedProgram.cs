using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Run;

sealed class BootstrappedProgram : IProgram
{
	readonly IProgram                   _previous;
	readonly Alter<LoggerConfiguration> _configuration;

	public BootstrappedProgram(IProgram previous, Alter<LoggerConfiguration> configuration)
	{
		_previous      = previous;
		_configuration = configuration;
	}

	public async Task Get(Array<string> parameter)
	{
		ILogger logger = _configuration(new LoggerConfiguration()).CreateBootstrapLogger();

		logger.Information("Starting up");

		try
		{
			await _previous.Await(parameter);
		}
		catch (Exception ex) when (ex is not HostAbortedException)
		{
			logger.Fatal(ex, "Unhandled exception");
		}
		finally
		{
			logger.Information("Shut down complete");
			logger.ToDisposable().Dispose();
		}
	}
}
using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class LoggingAwareProcessor : IStopAware<Guid>
{
	readonly IStopAware<Guid> _previous;
	readonly ErrorOccurred    _error;

	public LoggingAwareProcessor(IStopAware<Guid> previous, ILoggerFactory factory)
		: this(previous, factory.CreateLogger(previous.GetType())) {}

	public LoggingAwareProcessor(IStopAware<Guid> previous, ILogger logger)
		: this(previous, new ErrorOccurred(logger)) {}

	public LoggingAwareProcessor(IStopAware<Guid> previous, ErrorOccurred error)
	{
		_previous = previous;
		_error    = error;
	}

	public async ValueTask Get(Stop<Guid> parameter)
	{
		try
		{
			await _previous.Off(parameter);
		}
		catch (Exception e)
		{
			_error.Execute(e, parameter);
			throw;
		}
	}

	public sealed class ErrorOccurred : LogWarningException<Guid>
	{
		public ErrorOccurred(ILogger logger) : base(logger, "Exception encountered processing {Identity}") {}
	}
}
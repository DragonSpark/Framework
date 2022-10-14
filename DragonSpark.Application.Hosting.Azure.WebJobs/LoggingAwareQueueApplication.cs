using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

sealed class LoggingAwareQueueApplication : IOperation<Guid>
{
	readonly IOperation<Guid> _previous;
	readonly ErrorOccurred    _error;

	public LoggingAwareQueueApplication(IOperation<Guid> previous, ILoggerFactory factory)
		: this(previous, factory.CreateLogger(previous.GetType())) {}

	public LoggingAwareQueueApplication(IOperation<Guid> previous, ILogger logger)
		: this(previous, new ErrorOccurred(logger)) {}

	public LoggingAwareQueueApplication(IOperation<Guid> previous, ErrorOccurred error)
	{
		_previous = previous;
		_error    = error;
	}

	public async ValueTask Get(Guid parameter)
	{
		try
		{
			await _previous.Await(parameter);
		}
		catch (Exception e)
		{
			_error.Execute(e, parameter);
			throw;
		}
	}

	public sealed class ErrorOccurred : LogWarningException<Guid>
	{
		public ErrorOccurred(ILogger logger) : base(logger, "Exception encountered processing {Guid}") {}
	}
}